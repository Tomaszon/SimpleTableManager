using System.Text;

namespace SimpleTableManager.Services;

public partial class SmartConsole
{
	public const string HELP_COMMAND = "--help";

	public static string LAST_HELP_PLACEHOLDER => Localizer.TryLocalize<SmartConsole>("helpPlaceholder");

	private static string _lastHelp = LAST_HELP_PLACEHOLDER;

	private static int _insertIndex = 0;

	private static readonly StringBuilder _buffer = new();

	private const string _COMMAND_LINE_PREFIX = "STM > ";

	private static readonly HistoryList _commandHistory = new(Settings.Current.CommandHistoryLength);

	private static readonly AutoComplete _autoComplete = new();

	public static void Render(Document document)
	{
		Console.Clear();

		Renderer.Render(document);

		if (_lastHelp is { })
		{
			Console.Write("Help:\n");

			foreach (var c in _lastHelp)
			{
				Console.Write(c);
			}

			Console.WriteLine("\n");
			Console.WriteLine(new string('\n', Settings.Current.CommandHintRowCount));
		}

		Console.Write(_COMMAND_LINE_PREFIX);
	}

	public static string ReadLineWhile(string message, IEnumerable<object> validValues)
	{
		do
		{
			Console.WriteLine(message);

			if (Console.ReadLine() is string answer && validValues.Select(v => v.ToString()).Contains(answer, StringComparer.OrdinalIgnoreCase))
			{
				return answer;
			}
		}
		while (true);
	}

	public static void ShowHelp(string rawCommand, List<string>? availableKeys, CommandReference? commandReference, string error)
	{
		var command = new Command(commandReference, rawCommand, null) { AvailableKeys = availableKeys };

		_lastHelp = $"{error}\n    ";

		if (command.AvailableKeys is { })
		{
			_lastHelp += $"\nAvailable keys:\n";

			foreach (var key in command.AvailableKeys)
			{
				_lastHelp += $"        {key}";

				if (InstanceMap.Instance.TryGetInstances(key, out _, out var type) &&
					type.GetCustomAttribute<CommandInformationAttribute>() is var attribute &&
					attribute is not null)
				{
					_lastHelp += $":  {attribute.Information}";
				}

				_lastHelp += ",\n";
			}
		}
		else if (command.Reference.HasValue)
		{
			InstanceMap.Instance.GetInstances(command.Reference.Value.ClassName, out var type);

			var method = command.GetMethod(type);

			var parameters = Command.GetParameters(method, true);

			if (method.GetCustomAttribute<CommandInformationAttribute>()?.Information is var info && info is not null)
			{
				_lastHelp += $"Summary:\n        {info}\n    ";
			}

			_lastHelp += $"Parameters:\n        {(parameters.Count > 0 ? string.Join("\n        ", parameters) : "No parameters")}\n    ";

			if (method.GetCustomAttribute<CommandShortcutAttribute>()?.Key is var key && key is not null)
			{
				if (CommandShortcuts.TryGetShortcut(key, out var shortcut))
				{
					_lastHelp += $"Shortcut:\n        {(shortcut.Value.Item1 != ConsoleModifiers.None ? $"{shortcut.Value.Item1} + {shortcut.Value.Item2}" : shortcut.Value.Item2)}\n    ";
				}
			}
		}

		if (command.RawCommand.Replace(HELP_COMMAND, "").TrimEnd() is var sanitizedCommand &&
			!string.IsNullOrWhiteSpace(sanitizedCommand))
		{
			_lastHelp += $"in '{sanitizedCommand}'";
		}

		_lastHelp = _lastHelp.Trim();
	}

	public static void ShowResults(IEnumerable<object?> results)
	{
		results = results.Where(r => r is not null);

		if (results?.Count() > 0)
		{
			var formattedResults = results.Select(r => JsonConvert.SerializeObject(r, Formatting.Indented));

			_lastHelp = $"Execution result:\n";

			formattedResults.ForEach(r => _lastHelp += $"{r},\n");
		}
		else
		{
			_lastHelp = LAST_HELP_PLACEHOLDER;
		}

		_lastHelp = _lastHelp.TrimEnd(',', '\n');
	}

	public static string ReadInput(out Command? command)
	{
		ClearBuffer();

		bool saveToHistory;

		while (ReadInputKey(out saveToHistory, out command)) ;

		var rawCommand = _buffer.ToString().Trim();

		if (saveToHistory && !string.IsNullOrEmpty(rawCommand))
		{
			_commandHistory.Add(rawCommand);
		}
		else
		{
			_commandHistory.ResetCycle();
		}

		return rawCommand;
	}

	private static bool ReadInputKey(out bool saveToHistory, out Command? command)
	{
		var k = Console.ReadKey(true);

		if (TryReadCommandShortcut(k, out saveToHistory, out command))
		{
			return false;
		}

		return ReadInputChar(k, out saveToHistory);
	}

	private static bool TryReadCommandShortcut(ConsoleKeyInfo k, out bool saveToHistory, out Command? command)
	{
		saveToHistory = false;

		if (CommandShortcuts.TryGetAction(k, out var action))
		{
			if (action == HELP_COMMAND)
			{
				command = null;

				GetHelp(out saveToHistory);
			}
			else
			{
				(var type, var methods) = InstanceMap.Instance.GetTypes()
					.Select(t => (type: t, methods: CommandShortcuts.GetMethods(t)))
					.SingleOrDefault(p => p.methods.ContainsKey(action));

				if (type is null)
				{
					throw new InvalidOperationException($"Invalid value for shortcut '{k.Modifiers} + {k.Key}'");
				}

				var method = methods[action];

				command = new Command(new CommandReference(type.Name, method.Name), "", null);
			}

			_autoComplete.Reset();

			return true;
		}

		command = null;

		return false;
	}

	private static bool ReadInputChar(ConsoleKeyInfo k, out bool saveToHistory)
	{
		saveToHistory = true;

		ClearHintRows(true);

		return k.Key switch
		{
			ConsoleKey.Enter => AcceptCommand(),

			ConsoleKey.Tab => IterateHint(k.Modifiers),
			ConsoleKey.Backspace => DeleteCharsToLeft(k.Modifiers),
			ConsoleKey.Delete => DeleteCharsToRight(k.Modifiers),
			ConsoleKey.UpArrow => GetPreviousHistoryItem(),
			ConsoleKey.DownArrow => GetNextHistoryItem(),
			ConsoleKey.RightArrow => MoveCursorRight(k.Modifiers),
			ConsoleKey.LeftArrow => MoveCursorLeft(k.Modifiers),
			ConsoleKey.Home => MoveCursorToTheLeft(),
			ConsoleKey.End => MoveCursorToTheRight(),
			ConsoleKey.Escape => Escape(),

			_ => ManualInsertCharToBuffer(k.KeyChar)
		};
	}

	private static bool GetHelp(out bool saveToHistory)
	{
		$" {HELP_COMMAND}".ForEach(c => ManualInsertCharToBuffer(c));

		saveToHistory = false;

		return AcceptCommand();
	}

	private static (int top, int left) ClearHintRows(bool restoreCursorPosition)
	{
		(var left, var top) = Console.GetCursorPosition();

		MoveCursorToHintRow(top);

		Shared.IndexArray(Settings.Current.CommandHintRowCount).ForEach(i =>
			Console.WriteLine(new string(' ', Console.WindowWidth)));

		if (restoreCursorPosition)
		{
			Console.SetCursorPosition(left, top);
		}
		else
		{
			MoveCursorToHintRow(top);
		}

		return (left, top);
	}

	private static void MoveCursorToHintRow(int top)
	{
		Console.SetCursorPosition(0, top - Settings.Current.CommandHintRowCount - 1);
	}

	private static void ShowHintRows(List<(string key, bool isLeaf)> availableKeys, string nextKey, string? partialKey)
	{
		(var left, var top) = Console.GetCursorPosition();

		MoveCursorToHintRow(top);

		var keyIndexAggregate = 0;

		Shared.IndexArray(Settings.Current.CommandHintRowCount).ForEach(i =>
		{
			if (availableKeys.Count <= keyIndexAggregate)
			{
				return;
			}

			ShowHintRow(availableKeys.Skip(keyIndexAggregate).ToList(), nextKey, out var keyIndex, partialKey);
			keyIndexAggregate += keyIndex;

			Console.WriteLine();
		});

		if (keyIndexAggregate < availableKeys.Count)
		{
			Console.Write(nextKey == availableKeys[keyIndexAggregate - 1].key ? "..." : " ...");
		}

		Console.ForegroundColor = Settings.Current.TextColor.Foreground;

		Console.SetCursorPosition(left, top);
	}

	private static void ShowHintRow(List<(string key, bool isLeaf)> availableKeys, string nextKey, out int keyIndex, string? partialKey)
	{
		keyIndex = 0;

		if (nextKey != availableKeys[0].key)
		{
			Console.Write(" ");
		}
		while (keyIndex < availableKeys.Count &&
			Console.GetCursorPosition().Left < Console.WindowWidth - availableKeys[keyIndex].key.Length - 5)
		{
			var availableKey = availableKeys[keyIndex];

			var formattedAvailableKey = availableKey.isLeaf ?
				$"{availableKey.key}" : $"{availableKey.key}...";

			var key = nextKey == availableKey.key ?
				$"{Shared.BOLD_CHAR_CODE}[ {formattedAvailableKey} ]{Shared.NORMAL_CHAR_CODE}" :
				$" {formattedAvailableKey} ";
			if (keyIndex > 0 && nextKey != availableKey.key && nextKey != availableKeys[keyIndex - 1].key)
			{
				Console.Write(" ");
			}

			if (partialKey is not null && !availableKey.key.StartsWith(partialKey))
			{
				Console.ForegroundColor = Settings.Current.NotAvailableContentColor.Foreground;
			}
			else
			{
				Console.ForegroundColor = Settings.Current.TextColor.Foreground;
			}

			Console.Write(key);
			keyIndex++;
		}
	}

	private static bool IterateHint(ConsoleModifiers modifiers)
	{
		var nextKey = GetHint(modifiers, out _, out var isSpaceAppendNeeded, out var pacl, out var pk, out var pkl, out var aks);

		if (nextKey is not null)
		{
			if (aks is not null)
			{
				ShowHintRows(aks, nextKey, pk);
			}

			MoveCursorToTheRight();
			DeleteCharsToLeft(pacl + pkl);
			if (isSpaceAppendNeeded)
			{
				InsertCharToBuffer(' ');
			}
			InsertStringToBuffer(nextKey);
		}
		else
		{
			Play(Settings.Current.ErrorNotes);
		}

		return true;
	}

	private static bool ManualInsertCharToBuffer(char c)
	{
		_autoComplete.Reset();

		return InsertCharToBuffer(c);
	}

	private static bool AcceptCommand()
	{
		_autoComplete.Reset();

		Console.WriteLine();

		return false;
	}

	private static string? GetHint(ConsoleModifiers modifiers, out int keyCount, out bool isSpaceAppendNeeded, out int previousAutoCompleteLength, out string? partialKey, out int partialKeyLength, out List<(string key, bool isLeaf)>? availableKeys)
	{
		var value = _buffer.ToString();

		value = _autoComplete.TrimValueForCycling(value);

		var rawCommand = Get1stLevelHelpCommand(value);

		var result = GetHintCore(rawCommand, out availableKeys, out partialKey);

		if (result == GetHintResult.PartialKey || result == GetHintResult.Complete)
		{
			rawCommand = Get2ndLevelHelpCommand(value);

			result = GetHintCore(rawCommand, out availableKeys, out _);

			partialKey ??= value[(value.LastIndexOf(' ') + 1)..];
		}

		if (!_autoComplete.Cycling)
		{
			_autoComplete.SetKeys(availableKeys?.Select(p => p.key));
		}

		if (result == GetHintResult.Hint)
		{
			var nextKey = _autoComplete.GetNextKey(partialKey, modifiers == ConsoleModifiers.Shift, out previousAutoCompleteLength, out keyCount);

			isSpaceAppendNeeded = IsSpaceAppendNeeded(value, partialKey);

			partialKeyLength = partialKey?.Length ?? 0;

			return nextKey;
		}

		keyCount = 0;
		isSpaceAppendNeeded = false;
		previousAutoCompleteLength = 0;
		partialKeyLength = 0;

		return null;
	}

	private static GetHintResult GetHintCore(string command, out List<(string key, bool isLeaf)>? availableKeys, out string? partialKey)
	{
		try
		{
			Command.FromString(command);

			partialKey = null;
			availableKeys = null;

			return GetHintResult.Complete;
		}
		catch (HelpRequestedException ex)
		{
			availableKeys = ex.AvailableKeys?.SelectMany(k =>
				k.key.Split(Command._ALTERNATIVE_KEY_SEPARATOR).Select(p => (p, k.isLeaf))).OrderBy(p => p.p).ToList();

			partialKey = null;

			return ex.AvailableKeys is not null && ex.AvailableKeys.Count > 0 ? GetHintResult.Hint : GetHintResult.Complete;
		}
		catch (PartialKeyException ex)
		{
			availableKeys = null;

			partialKey = ex.PartialKey;

			return GetHintResult.PartialKey;
		}
		catch (Exception e) when (e is KeyNotFoundException || e is CommandKeyNotFoundException)
		{
			availableKeys = null;

			partialKey = null;

			return GetHintResult.UnknownKey;
		}

		throw new InvalidOperationException();
	}

	private static string Get1stLevelHelpCommand(string value)
	{
		return string.IsNullOrWhiteSpace(value) ? HELP_COMMAND : $"{value} {HELP_COMMAND}";
	}

	private static string Get2ndLevelHelpCommand(string value)
	{
		return !value.Contains(' ') ? HELP_COMMAND : $"{value[..value.LastIndexOf(' ')]} {HELP_COMMAND}";
	}

	private static bool IsSpaceAppendNeeded(string value, string? partialKey)
	{
		return partialKey is null &&
			!string.IsNullOrWhiteSpace(value) &&
			!value.EndsWith(' ');
	}

	private static bool GetPreviousHistoryItem()
	{
		if (_commandHistory.TryGetPreviousHistoryItem(out var command))
		{
			ClearBuffer();

			_autoComplete.Reset();

			InsertStringToBuffer(command);
		}

		return true;
	}

	private static bool GetNextHistoryItem()
	{
		if (_commandHistory.TryGetNextHistoryItem(out var command))
		{
			ClearBuffer();

			_autoComplete.Reset();

			InsertStringToBuffer(command);
		}

		return true;
	}

	private static bool DeleteCharToLeft()
	{
		if (_insertIndex > 0)
		{
			Shared.StepCursor(-1, 0);
			var clearLength = _buffer.Length - _insertIndex + 1;
			_buffer.Remove(_insertIndex - 1, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);
			_insertIndex--;

			var rest = _buffer.ToString()[_insertIndex..];
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool DeleteCharsToLeft(ConsoleModifiers modifiers)
	{
		_autoComplete.Reset();

		DeleteCharToLeft();

		if (modifiers == ConsoleModifiers.Control)
		{
			while (!IsLeftWordBorder())
			{
				DeleteCharToLeft();
			}
		}

		return true;
	}

	private static bool DeleteCharToRight()
	{
		if (_insertIndex < _buffer.Length)
		{
			var clearLength = _buffer.Length - _insertIndex;
			_buffer.Remove(_insertIndex, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);

			var rest = _buffer.ToString()[_insertIndex..];
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool DeleteCharsToRight(ConsoleModifiers modifiers)
	{
		_autoComplete.Reset();

		DeleteCharToRight();

		if (modifiers == ConsoleModifiers.Control)
		{
			while (!IsRightWordBorder())
			{
				DeleteCharToRight();
			}
		}

		return true;
	}

	private static bool MoveCursorLeft(ConsoleModifiers modifiers = default)
	{
		switch (modifiers)
		{
			case ConsoleModifiers.Control:
				{
					do
					{
						MoveCursorLeft();
					}
					while (!IsLeftWordBorder());
				}
				break;

			default:
				{
					if (_insertIndex > 0)
					{
						Shared.StepCursor(-1, 0);
						_insertIndex--;
					}
				}
				break;
		}

		return true;
	}

	private static bool MoveCursorRight(ConsoleModifiers modifiers = default)
	{
		switch (modifiers)
		{
			case ConsoleModifiers.Control:
				{
					do
					{
						MoveCursorRight();
					}
					while (!IsRightWordBorder());
				}
				break;

			default:
				{
					if (_insertIndex < _buffer.Length)
					{
						Shared.StepCursor(1, 0);
						_insertIndex++;
					}
				}
				break;
		}

		return true;
	}

	private static bool IsRightWordBorder()
	{
		var charAtIndex = CharAtIndex();
		var charAfterIndex = CharAtIndex(1);

		return !CanCursorMoveRight() || charAtIndex == ' ' && charAfterIndex != ' ';
	}

	private static bool IsLeftWordBorder()
	{
		var charBeforeIndex = CharAtIndex(-1);
		var charBeforeThat = CharAtIndex(-2);

		return !CanCursorMoveLeft() || charBeforeIndex == ' ' && charBeforeThat != ' ';
	}

	private static bool CanCursorMoveLeft()
	{
		return _insertIndex > 0;
	}

	private static bool CanCursorMoveRight()
	{
		return _insertIndex < _buffer.Length;
	}

	private static char CharAtIndex(int offset = 0)
	{
		return _insertIndex + offset >= 0 && _insertIndex + offset < _buffer.Length ? _buffer[_insertIndex + offset] : default;
	}

	private static bool MoveCursorToTheLeft()
	{
		while (CanCursorMoveLeft())
		{
			MoveCursorLeft();
		}

		return true;
	}

	private static bool MoveCursorToTheRight()
	{
		while (CanCursorMoveRight())
		{
			MoveCursorRight();
		}

		return true;
	}

	private static void DeleteCharsToLeft(int length)
	{
		for (int i = 0; i < length; i++)
		{
			DeleteCharToLeft();
		}
	}

	private static void InsertStringToBuffer(string? s)
	{
		if (s is not null)
		{
			foreach (var c in s)
			{
				InsertCharToBuffer(c);
			}
		}
	}

	private static bool InsertCharToBuffer(char c)
	{
		if (c != '\0')
		{
			Console.Write(c);
			_buffer.Insert(_insertIndex, c);
			_insertIndex++;
			var rest = _buffer.ToString()[_insertIndex..];
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool Escape()
	{
		ClearBuffer();

		_commandHistory.ResetCycle();

		return true;
	}

	private static bool ClearBuffer()
	{
		Console.SetCursorPosition(_COMMAND_LINE_PREFIX.Length, Console.CursorTop);
		Console.Write(new string(' ', _buffer.Length));
		Console.SetCursorPosition(_COMMAND_LINE_PREFIX.Length, Console.CursorTop);

		_buffer.Clear();
		_insertIndex = 0;

		return true;
	}

	public static async void Play(Note[] notes)
	{
		if (Settings.Current.Audio)
		{
			SmartPlayer player = new(notes);

			await player.Play();
		}
	}
}