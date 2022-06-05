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

	private static int _rawCommandHistoryIndex = -1;

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
		int instertIndex = 0;
		StringBuilder buffer = new StringBuilder();

		while (Console.ReadKey(false) is var k)
		{
			switch (k.Key)
			{
				case ConsoleKey.Enter:
					{
						var rawCommand = buffer.ToString().Trim();

						rawCommandHistory.Add(rawCommand);

						if (rawCommandHistory.Count > _commandHistoryLength)
						{
							rawCommandHistory.RemoveAt(0);
						}

						_rawCommandHistoryIndex = rawCommandHistory.Count - 1;

						return rawCommand;
					}

				case ConsoleKey.Tab:
					{
						//lastHelp = "WEEE";
						//Console.Clear();
						//TableRenderer.Render(table, viewOptions);

						//if (lastHelp is { })
						//{
						//	Console.WriteLine($"Help: {lastHelp}");
						//}
						//Console.Write("> ");

						//break;

						Console.SetCursorPosition(Console.CursorLeft - 8, Console.CursorTop);

					}
					break;

				case ConsoleKey.Backspace:
					{
						DeleteCharToLeft(buffer, ref instertIndex);
					}
					break;

				case ConsoleKey.Delete:
					{
						DeleteCharToRight(buffer, ref instertIndex);
					}
					break;

				case ConsoleKey.UpArrow:
					{
						if (_rawCommandHistoryIndex >= 0)
						{
							Console.SetCursorPosition(_COMMAND_LINE_PREFIX.Length, Console.CursorTop);
							Console.Write(new string(' ', buffer.Length));
							Console.SetCursorPosition(_COMMAND_LINE_PREFIX.Length, Console.CursorTop);
							Console.Write(rawCommandHistory[_rawCommandHistoryIndex]);
							buffer.Clear();
							instertIndex = 0;
							InsterStringToBuffer(buffer, ref instertIndex, rawCommandHistory[_rawCommandHistoryIndex]);
							_rawCommandHistoryIndex--;
						}
					}
					break;

				// case ConsoleKey.DownArrow:
				// 	{
				// 		Console.SetCursorPosition(Console.CursorLeft - buffer.Length, Console.CursorTop);
				// 		Console.Write(new string(' ', buffer.Length));
				// 		Console.SetCursorPosition(Console.CursorLeft - buffer.Length, Console.CursorTop);
				// 		buffer = "";

				// 	}
				// 	break;

				case ConsoleKey.RightArrow:
					{
						MoveCursorRight(buffer, ref instertIndex);
					}
					break;

				case ConsoleKey.LeftArrow:
					{
						MoveCursorLeft(buffer, ref instertIndex);
					}
					break;

				case ConsoleKey.Home:
					{
						MoveCursorToTheLeft(buffer, ref instertIndex);
					}
					break;

				case ConsoleKey.End:
					{
						MoveCursorToTheRight(buffer, ref instertIndex);
					}
					break;

				default:
					{
						InsterCharToBuffer(buffer, ref instertIndex, k.KeyChar);
					}
					break;
			};
		}

		return "¯\\_(ツ)_/¯";
	}

	private static void DeleteCharToLeft(StringBuilder buffer, ref int instertIndex)
	{
		if (instertIndex > 0)
		{
			var clearLength = buffer.Length - instertIndex + 1;
			buffer.Remove(instertIndex - 1, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);
			instertIndex--;

			var rest = buffer.ToString().Substring(instertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}
		else
		{
			Shared.StepCursor(1, 0);
		}
	}

	private static void DeleteCharToRight(StringBuilder buffer, ref int instertIndex)
	{
		if (instertIndex < buffer.Length)
		{
			var clearLength = buffer.Length - instertIndex + 1;
			buffer.Remove(instertIndex, 1);
			Console.Write(new string(' ', clearLength));
			Shared.StepCursor(-clearLength, 0);

			var rest = buffer.ToString().Substring(instertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}
	}

	private static bool MoveCursorLeft(StringBuilder buffer, ref int instertIndex)
	{
		if (instertIndex > 0)
		{
			Shared.StepCursor(-1, 0);
			instertIndex--;

			return true;
		}

		return false;
	}

	private static bool MoveCursorRight(StringBuilder buffer, ref int instertIndex)
	{
		if (instertIndex < buffer.Length)
		{
			Shared.StepCursor(1, 0);
			instertIndex++;

			return true;
		}

		return false;
	}

	private static void MoveCursorToTheLeft(StringBuilder buffer, ref int instertIndex)
	{
		while(MoveCursorLeft(buffer, ref instertIndex));
	}

	private static void MoveCursorToTheRight(StringBuilder buffer, ref int instertIndex)
	{
		while(MoveCursorRight(buffer, ref instertIndex));
	}

	private static void InsterStringToBuffer(StringBuilder buffer, ref int instertIndex, string s)
	{
		foreach (var c in s)
		{
			InsterCharToBuffer(buffer, ref instertIndex, c);
		}
	}

	private static void InsterCharToBuffer(StringBuilder buffer, ref int instertIndex, char c)
	{
		if (c != '\0')
		{
			buffer.Insert(instertIndex, c);
			instertIndex++;
			var rest = buffer.ToString().Substring(instertIndex);
			Console.Write(rest);
			Shared.StepCursor(-rest.Length, 0);
		}
	}
}
