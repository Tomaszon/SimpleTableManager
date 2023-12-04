using System.Formats.Asn1;
using System.Text;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public partial class SmartConsole
{
	public const string HELP_COMMAND = "help";

	private static readonly string _LAST_HELP_PLACEHOLDER = "Enter command to execute";

	private static string _lastHelp = _LAST_HELP_PLACEHOLDER;

	private static int _insertIndex = 0;

	private static readonly StringBuilder _buffer = new();

	private const string _COMMAND_LINE_PREFIX = "> ";

	private static readonly CommandHistory _commandHistory = new();

	private static readonly AutoComplete _autoComplete = new();

	public static void Render(Document document)
	{
		Renderer.Render(document);

		if (_lastHelp is { })
		{
			Console.Write("Help:\n");

			foreach (var c in _lastHelp)
			{
				Console.Write(c);
			}

			Console.WriteLine("\n");
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
			_lastHelp += $"Available keys:\n";

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
		else if (command.Reference is { })
		{
			InstanceMap.Instance.GetInstances(command.Reference.ClassName, out var type);

			var method = command.GetMethod(type);

			var parameters = Command.GetParameters(method);

			if (method.GetCustomAttribute<CommandInformationAttribute>()?.Information is var info && info is not null)
			{
				_lastHelp += $"Summary:\n        {info}\n    ";
			}

			_lastHelp += $"Parameters:\n        {(parameters.Count > 0 ? string.Join("\n        ", parameters) : "No parameters")}\n";
		}

		if (command.RawCommand.Replace(HELP_COMMAND, "").TrimEnd() is var sanitedCommand &&
			!string.IsNullOrWhiteSpace(sanitedCommand))
		{
			_lastHelp += $"    in '{sanitedCommand}'";
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
			_lastHelp = _LAST_HELP_PLACEHOLDER;
		}

		_lastHelp = _lastHelp.TrimEnd(',', '\n');
	}

	public static string ReadInputString()
	{
		ClearBuffer();

		while (ReadInputChar()) ;

		var command = _buffer.ToString().Trim();

		_commandHistory.Add(command);

		return command;
	}

	private static bool ReadInputChar()
	{
		var k = Console.ReadKey(true);

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

	private static bool IterateHint(ConsoleModifiers modifiers)
	{
		var nextKey = GetHint(modifiers, out _, out var isSpaceAppendNeeded, out var pacl, out var pkl);

		if (nextKey is not null)
		{
			MoveCursorToTheRight();
			DeleteCharsToLeft(pacl + pkl);
			if (isSpaceAppendNeeded)
			{
				InsertCharToBuffer(' ');
			}
			InsertStringToBuffer(nextKey);
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

	private static string? GetHint(ConsoleModifiers modifiers, out int keyCount, out bool isSpaceAppendNeeded, out int prevoiusAutoCompleteLength, out int partialKeyLength)
	{
		var value = _buffer.ToString();

		value = _autoComplete.TrimValueForCycling(value);

		var rawCommand = Get1stLevelHelpCommand(value);

		var result = GetHintCore(rawCommand, out var availableKeys, out var partialKey);

		if (result == GetHintResult.PartialKey || result == GetHintResult.Complete)
		{
			rawCommand = Get2ndLevelHelpCommand(value);

			result = GetHintCore(rawCommand, out availableKeys, out _);

			partialKey ??= value[(value.LastIndexOf(' ') + 1)..];
		}

		if (!_autoComplete.Cycling)
		{
			_autoComplete.SetKeys(availableKeys);
		}

		if (result == GetHintResult.Hint)
		{
			var nextKey = _autoComplete.GetNextKey(partialKey, modifiers.HasFlag(ConsoleModifiers.Shift), out prevoiusAutoCompleteLength, out keyCount);

			isSpaceAppendNeeded = IsSpaceAppendNeeded(value, partialKey);

			partialKeyLength = partialKey?.Length ?? 0;

			return nextKey;
		}

		keyCount = 0;
		isSpaceAppendNeeded = false;
		prevoiusAutoCompleteLength = 0;
		partialKeyLength = 0;

		return null;
	}

	private static GetHintResult GetHintCore(string command, out List<string>? availableKeys, out string? partialKey)
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
			availableKeys = ex.AvailableKeys;

			partialKey = null;

			return ex.AvailableKeys is not null && ex.AvailableKeys.Count > 0 ? GetHintResult.Hint : GetHintResult.Complete;
		}
		catch (PartialKeyException ex)
		{
			availableKeys = null;

			partialKey = ex.PartialKey;

			return GetHintResult.PartialKey;
		}
		catch (KeyNotFoundException)
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
		if (modifiers.HasFlag(ConsoleModifiers.Control))
		{
			do
			{
				MoveCursorLeft();
			}
			while (!IsLeftWordBorder());
		}
		else
		{
			if (_insertIndex > 0)
			{
				Shared.StepCursor(-1, 0);
				_insertIndex--;
			}
		}

		return true;
	}

	private static bool MoveCursorRight(ConsoleModifiers modifiers = default)
	{
		if (modifiers.HasFlag(ConsoleModifiers.Control))
		{
			do
			{
				MoveCursorRight();
			}
			while (!IsRightWordBorder());
		}
		else
		{
			if (_insertIndex < _buffer.Length)
			{
				Shared.StepCursor(1, 0);
				_insertIndex++;
			}
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
}