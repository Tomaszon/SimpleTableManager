using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public class SmartConsole
{
	public static string LastHelp = "Enter command to execute";

	private static int _insertIndex = 0;

	private static StringBuilder _buffer = new StringBuilder();

	private const string _COMMAND_LINE_PREFIX = "> ";

	// private static int _autoCompleteLength = 0;

	// private static int _autoCompleteIndex = -1;

	// private static string _autoCompletePartialRawCommand;

	private static CommandHistory _commandHistory = new CommandHistory();

	private static AutoComplete _autoComplete = new AutoComplete();

	public static void Render(Document document)
	{
		Renderer.Render(document);

		if (LastHelp is { })
		{
			Console.Write("Help:\n");

			foreach (var c in LastHelp)
			{
				Console.Write(c);

				Task.Delay(10).Wait();
			}

			Console.WriteLine("\n");
		}

		Console.Write(_COMMAND_LINE_PREFIX);
	}

	public static void ShowHelp(string rawCommand, List<string> availableKeys, CommandReference commandReference, string error)
	{
		var command = new Command() { AvailableKeys = availableKeys, RawCommand = rawCommand, Reference = commandReference };

		LastHelp = $"{error}\n    ";

		if (command.AvailableKeys is { })
		{
			LastHelp += $"Available keys:\n";

			foreach (var key in command.AvailableKeys)
			{
				LastHelp += $"        {key}";

				if (InstanceMap.Instance.TryGetInstances(key, out _, out var type) &&
					type.GetCustomAttribute<CommandInformationAttribute>() is var attribute &&
					attribute is not null)
				{
					LastHelp += $":  {attribute.Information}";
				}

				LastHelp += ",\n";
			}
		}
		else if (command.Reference is { })
		{
			InstanceMap.Instance.GetInstances(command.Reference.ClassName, out var type);

			var method = command.GetMethod(type);

			var parameters = command.GetParameters(method);

			if (method.GetCustomAttribute<CommandInformationAttribute>().Information is var info && info is not null)
			{
				LastHelp += $"Summary:\n        {info}\n    ";
			}

			LastHelp += $"Parameters:\n        {(parameters.Count > 0 ? string.Join("\n        ", parameters) : "No parameters")}\n    of ";
		}

		if (command.RawCommand.Replace(Shared.HELP_COMMAND, "").TrimEnd() is var sanitedCommand &&
			!string.IsNullOrWhiteSpace(sanitedCommand))
		{
			LastHelp += $"    in '{sanitedCommand}'";
		}

		LastHelp = LastHelp.Trim();
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

			ConsoleKey.Tab => GetHint(k.Modifiers),
			ConsoleKey.Backspace => ManualDeleteCharToLeft(),
			ConsoleKey.Delete => ManualDeleteCharToRight(),
			ConsoleKey.UpArrow => GetPreviousHistoryItem(),
			ConsoleKey.DownArrow => GetNextHistoryItem(),
			ConsoleKey.RightArrow => MoveCursorRight() || true,
			ConsoleKey.LeftArrow => MoveCursorLeft() || true,
			ConsoleKey.Home => MoveCursorToTheLeft(),
			ConsoleKey.End => MoveCursorToTheRight(),
			ConsoleKey.Escape => Escape(),

			_ => ManualInsertCharToBuffer(k.KeyChar)
		};
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

	private static bool GetHint(ConsoleModifiers modifiers)
	{
		var value = _buffer.ToString();

		value = value.Substring(0, value.Length - _autoComplete.AutoCompleteLength);

		var rawCommand = $"{value} help";

		// while (true)
		// {
		var result = GetHintCore(rawCommand, out var availableKeys);

		if (!_autoComplete.Cycling)
		{
			_autoComplete.SetKeys(availableKeys);
		}

		switch (result)
		{
			case GetHintResult.Hint:
				{
					var nextKey = _autoComplete.GetNextKey(null, modifiers.HasFlag(ConsoleModifiers.Shift), out var pacl);

					MoveCursorToTheRight();
					DeleteCharsToLeft(pacl);
					InsertStringToBuffer(nextKey);
				}
				break;

			case GetHintResult.PartialKey:
				{

				}
				break;

				// case GetHintResult.Complete: return true;

				// case GetHintResult.PartialKey:
				// 	{

				// 	}
				// 	break;

				// default:
				// 	{
				// 		if (rawCommand.Count(c => c == ' ') > 1)
				// 		{
				// 			var a = rawCommand.Substring(0, rawCommand.Length - 5);
				// 			var b = a.Substring(0, a.LastIndexOf(' '));
				// 			rawCommand = $"{b} help";
				// 		}
				// 		else
				// 		{
				// 			rawCommand = "help";
				// 		}
				// 	}
				// 	break;
				//}
		}

		return true;
	}

	private static GetHintResult GetHintCore(string command, out List<string> availableKeys)
	{
		try
		{
			Command.FromString(command);
		}
		catch (HelpRequestedException ex)
		{
			availableKeys = ex.AvailableKeys;

			return ex.AvailableKeys is not null && ex.AvailableKeys.Count > 0 ? GetHintResult.Hint : GetHintResult.Complete;
		}
		catch (PartialKeyException)
		{
			availableKeys = null;

			return GetHintResult.PartialKey;
		}
		catch (KeyNotFoundException)
		{
			availableKeys = null;

			return GetHintResult.UnknownKey;
		}

		throw new InvalidOperationException();
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

			var rest = _buffer.ToString().Substring(_insertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool ManualDeleteCharToLeft()
	{
		_autoComplete.Reset();

		return DeleteCharToLeft();
	}

	private static bool DeleteCharToRight()
	{
		if (_insertIndex < _buffer.Length)
		{
			var clearLength = _buffer.Length - _insertIndex;
			_buffer.Remove(_insertIndex, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);

			var rest = _buffer.ToString().Substring(_insertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool ManualDeleteCharToRight()
	{
		_autoComplete.Reset();

		return DeleteCharToRight();
	}

	private static bool MoveCursorLeft()
	{
		if (_insertIndex > 0)
		{
			Shared.StepCursor(-1, 0);
			_insertIndex--;

			return true;
		}

		return false;
	}

	private static bool MoveCursorRight()
	{
		if (_insertIndex < _buffer.Length)
		{
			Shared.StepCursor(1, 0);
			_insertIndex++;

			return true;
		}

		return false;
	}

	private static bool MoveCursorToTheLeft()
	{
		while (MoveCursorLeft()) ;

		return true;
	}

	private static bool MoveCursorToTheRight()
	{
		while (MoveCursorRight()) ;

		return true;
	}

	private static void DeleteCharsToLeft(int length)
	{
		for (int i = 0; i < length; i++)
		{
			DeleteCharToLeft();
		}
	}

	private static void InsertStringToBuffer(string s)
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
			var rest = _buffer.ToString().Substring(_insertIndex);
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

	public enum GetHintResult
	{
		Hint,
		Complete,
		PartialKey,
		UnknownKey
	}
}
