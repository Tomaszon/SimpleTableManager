using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleTableManager.Models;
using SimpleTableManager.Services;

namespace SimpleTableManager
{
	internal class Program
	{
		public static string lastHelp = "Enter command to execute";
		//public static string prevCommand = "";

		private static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");

			TableBorderCharacters.FromJson($@".\Configs\tableBorderCharacters.json");
			CommandTree.FromJson($@".\Configs\commands.json");

			ViewOptions viewOptions = null;// new ViewOptions(1, 1, 2, 3);

			var table = new Table("", 3, 4);

			//foreach (var cell in table.Cells)
			//{
			//if (cell.Position.X % 3 == 0)
			//{
			//cell.ForegroundColor = ConsoleColor.Black;
			//cell.BackgroundColor = ConsoleColor.White;
			//}
			//}


			//table[1, 0].SetContent("T");
			//table[1, 0].VertialAlignment = VertialAlignment.Top;
			//table[1, 1].SetContent("C");
			//table[1, 1].VertialAlignment = VertialAlignment.Center;
			//table[1, 2].SetContent("C");
			//table[1, 2].VertialAlignment = VertialAlignment.Center;
			//table[1, 3].SetContent("B");
			//table[1, 3].VertialAlignment = VertialAlignment.Bottom;
			//table[1, 0].HorizontalAlignment = HorizontalAlignment.Right;
			//table[2, 0].Content = "AAAAAAAAAAA";
			//table.SetColumnWidth(1, 0);
			//table[0, 0].GivenSize = new Size(25, 1);

			//Console.WriteLine(JsonConvert.SerializeObject(viewOptions));

			do
			{
				try
				{
					Draw(table, viewOptions);

					var rawCommand = Console.ReadLine().Trim();//ReadInput();

					if (rawCommand == "exit")
					{
						break;
					}
					else if (rawCommand.Contains("-help"))
					{
						GetHelp(table, rawCommand);
					}
					else if (rawCommand == "refresh")
					{
						rawCommand = "";
						Console.Clear();
					}
					else
					{
						try
						{
							var command = Command.FromString(rawCommand);

							command.Execute(table);

							lastHelp = "Enter command to execute";
						}
						catch (IncompleteCommandException)
						{
							GetHelp(table, $"{rawCommand} -help", "Incomplete command");
						}
						catch (TargetParameterCountException)
						{
							GetHelp(table, $"{rawCommand} -help", "Parameters needed");
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.Write("Press any key to continue");
					Console.ReadKey();
				}

				Console.Clear();
			}
			while (true);

			Console.WriteLine("Good bye!");

			Console.ReadKey();
		}

		public static void GetHelp(Table table, string rawCommand, string error = null)
		{
			var command = Command.FromString(rawCommand);

			if (command.AvailableKeys is { })
			{
				lastHelp = $"{error} Available keys: '{string.Join(", ", command.AvailableKeys)}' in '{rawCommand.Replace("-help", "").TrimEnd()}'".Trim();
			}
			else if (command.Reference is { })
			{
				var parameters = command.GetParameters(command.GetMethod(table));

				lastHelp = $"{error} Parameters: '{string.Join(", ", parameters)}' of '{rawCommand.Replace("-help", "").TrimEnd()}'".Trim();
			}
		}

		public static void Draw(Table table, ViewOptions viewOptions)
		{
			Console.WriteLine("Table 1\n");

			TableRenderer.Render(table, viewOptions);

			if (lastHelp is { })
			{
				Console.WriteLine($"Help: {lastHelp}\n");
			}

			Console.Write("> ");
		}

		//public static string ReadInput()
		//{
		//	var buffer = "";

		//	while (Console.ReadKey(false) is var k)
		//	{
		//		if (k.Key == ConsoleKey.Enter)
		//		{
		//			break;
		//		}
		//		else if (k.Key == ConsoleKey.Tab)
		//		{
		//			//lastHelp = "WEEE";
		//			//Console.Clear();
		//			//TableRenderer.Render(table, viewOptions);

		//			//if (lastHelp is { })
		//			//{
		//			//	Console.WriteLine($"Help: {lastHelp}");
		//			//}
		//			//Console.Write("> ");

		//			//break;

		//			Console.SetCursorPosition(Console.CursorLeft - 8, Console.CursorTop);

		//		}
		//		else if (k.Key == ConsoleKey.Backspace)
		//		{
		//			if (buffer.Length > 0)
		//			{
		//				buffer = buffer[0..^1];
		//				Console.Write(" ");
		//				Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
		//			}
		//			else
		//			{
		//				Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
		//			}
		//		}
		//		else if (k.Key == ConsoleKey.UpArrow)
		//		{
		//			Console.SetCursorPosition(Console.CursorLeft - buffer.Length, Console.CursorTop);
		//			Console.Write(new string(' ', buffer.Length));
		//			Console.SetCursorPosition(Console.CursorLeft - buffer.Length, Console.CursorTop);
		//			Console.Write(prevCommand);
		//			buffer = prevCommand;
		//		}
		//		else if (k.Key == ConsoleKey.DownArrow)
		//		{
		//			Console.SetCursorPosition(Console.CursorLeft - buffer.Length, Console.CursorTop);
		//			Console.Write(new string(' ', buffer.Length));
		//			Console.SetCursorPosition(Console.CursorLeft - buffer.Length, Console.CursorTop);
		//			buffer = "";
		//		}
		//		else
		//		{
		//			buffer += k.KeyChar;
		//		}
		//	}

		//	prevCommand = buffer;
		//	return buffer.Trim();
		//}
	}
}
