using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public class SmartConsole
{
	public static string LastHelp = "Enter command to execute";

	private const string _COMMAND_LINE_PREFIX = "> ";

	public static List<string> rawCommandHistory = new List<string>();

	private static int _commandHistoryLength = 3;

	private static int _rawCommandHistoryIndex = 0;

	public static void Draw(Document document)
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

	public static void ShowHelp(Command command, string error)
	{
		if (command.AvailableKeys is { })
		{
			LastHelp = $"{error}\n    Available keys:\n        {string.Join("\n        ", command.AvailableKeys)}\n    in '{command.RawCommand.Replace(Shared.HELP_COMMAND, "").TrimEnd()}'".Trim();
		}
		else if (command.Reference is { })
		{
			var instances = Program.InstanceMap.GetInstances(command.Reference.ClassName);

			var parameters = command.GetParameters(command.GetMethod(instances.First()));

			LastHelp = $"{error}\n    Parameters:\n        {(parameters.Count > 0 ? string.Join("\n        ", parameters) : "No parameters")}\n    of '{command.RawCommand.Replace(Shared.HELP_COMMAND, "").TrimEnd()}'".Trim();
		}
	}

	public static void ShowHelp(string rawCommand, List<string> availableKeys, CommandReference commandReference, string error)
	{
		ShowHelp(new Command() { AvailableKeys = availableKeys, RawCommand = rawCommand, Reference = commandReference }, error);
	}

	public static string ReadInput()
	{
		int insertIndex = 0;
		StringBuilder buffer = new StringBuilder();

		while (ReadInputChar(buffer, ref insertIndex)) ;

		var rawCommand = buffer.ToString().Trim();

		rawCommandHistory.Add(rawCommand);

		if (rawCommandHistory.Count > _commandHistoryLength)
		{
			rawCommandHistory.RemoveAt(0);
		}

		_rawCommandHistoryIndex = rawCommandHistory.Count;

		return rawCommand;
	}

	private static bool ReadInputChar(StringBuilder buffer, ref int insertIndex)
	{
		var k = Console.ReadKey(true);

		return k.Key switch
		{
			ConsoleKey.Enter => AcceptCommand(),

			//ConsoleKey.Tab => true, //TODO autocomplete
			ConsoleKey.Backspace => DeleteCharToLeft(buffer, ref insertIndex),
			ConsoleKey.Delete => DeleteCharToRight(buffer, ref insertIndex),
			//ConsoleKey.UpArrow => GetPreviousHistoryItem(buffer, ref insertIndex), //TODO fix
			//ConsoleKey.DownArrow => GetNextHistoryItem(buffer, ref insertIndex),
			ConsoleKey.RightArrow => MoveCursorRight(buffer, ref insertIndex) || true,
			ConsoleKey.LeftArrow => MoveCursorLeft(buffer, ref insertIndex) || true,
			ConsoleKey.Home => MoveCursorToTheLeft(buffer, ref insertIndex),
			ConsoleKey.End => MoveCursorToTheRight(buffer, ref insertIndex),
			ConsoleKey.Escape => ClearBuffer(buffer, ref insertIndex),

			_ => InsterCharToBuffer(buffer, ref insertIndex, k.KeyChar)
		};
	}

	private static bool AcceptCommand()
	{
		Console.WriteLine();

		return false;
	}

	private static bool GetPreviousHistoryItem(StringBuilder buffer, ref int insertIndex)
	{
		if (_rawCommandHistoryIndex > 0)
		{
			ClearBuffer(buffer, ref insertIndex);

			_rawCommandHistoryIndex--;

			Console.Write(rawCommandHistory[_rawCommandHistoryIndex]);
			InsterStringToBuffer(buffer, ref insertIndex, rawCommandHistory[_rawCommandHistoryIndex]);
		}

		return true;
	}

	private static bool GetNextHistoryItem(StringBuilder buffer, ref int insertIndex)
	{
		if (_rawCommandHistoryIndex < rawCommandHistory.Count)
		{
			ClearBuffer(buffer, ref insertIndex);

			_rawCommandHistoryIndex++;

			if (_rawCommandHistoryIndex < rawCommandHistory.Count)
			{
				Console.Write(rawCommandHistory[_rawCommandHistoryIndex]);
				InsterStringToBuffer(buffer, ref insertIndex, rawCommandHistory[_rawCommandHistoryIndex]);
			}
		}

		return true;
	}

	private static bool DeleteCharToLeft(StringBuilder buffer, ref int insertIndex)
	{
		if (insertIndex > 0)
		{
			Shared.StepCursor(-1, 0);
			var clearLength = buffer.Length - insertIndex + 1;
			buffer.Remove(insertIndex - 1, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);
			insertIndex--;

			var rest = buffer.ToString().Substring(insertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool DeleteCharToRight(StringBuilder buffer, ref int insertIndex)
	{
		if (insertIndex < buffer.Length)
		{
			var clearLength = buffer.Length - insertIndex;
			buffer.Remove(insertIndex, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);

			var rest = buffer.ToString().Substring(insertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool MoveCursorLeft(StringBuilder buffer, ref int insertIndex)
	{
		if (insertIndex > 0)
		{
			Shared.StepCursor(-1, 0);
			insertIndex--;

			return true;
		}

		return false;
	}

	private static bool MoveCursorRight(StringBuilder buffer, ref int insertIndex)
	{
		if (insertIndex < buffer.Length)
		{
			Shared.StepCursor(1, 0);
			insertIndex++;

			return true;
		}

		return false;
	}

	private static bool MoveCursorToTheLeft(StringBuilder buffer, ref int insertIndex)
	{
		while (MoveCursorLeft(buffer, ref insertIndex)) ;

		return true;
	}

	private static bool MoveCursorToTheRight(StringBuilder buffer, ref int insertIndex)
	{
		while (MoveCursorRight(buffer, ref insertIndex)) ;

		return true;
	}

	private static void InsterStringToBuffer(StringBuilder buffer, ref int insertIndex, string s)
	{
		foreach (var c in s)
		{
			InsterCharToBuffer(buffer, ref insertIndex, c);
		}
	}

	private static bool InsterCharToBuffer(StringBuilder buffer, ref int insertIndex, char c)
	{
		if (c != '\0')
		{
			Console.Write(c);
			buffer.Insert(insertIndex, c);
			insertIndex++;
			var rest = buffer.ToString().Substring(insertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}

		return true;
	}

	private static bool ClearBuffer(StringBuilder buffer, ref int insertIndex)
	{
		Console.SetCursorPosition(_COMMAND_LINE_PREFIX.Length, Console.CursorTop);
		Console.Write(new string(' ', buffer.Length));
		Console.SetCursorPosition(_COMMAND_LINE_PREFIX.Length, Console.CursorTop);

		buffer.Clear();
		insertIndex = 0;

		return true;
	}
}
