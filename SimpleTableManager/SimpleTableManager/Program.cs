using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using SimpleTableManager.Models;
using SimpleTableManager.Services;

namespace SimpleTableManager
{
	internal class Program
	{
		public static string lastHelp = "Enter command to execute";
		//public static string prevCommand = "";

		static InstanceMap InstanceMap { get; set; } = new InstanceMap();


		private static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");
			Console.OutputEncoding = System.Text.Encoding.Unicode;
			Console.InputEncoding = System.Text.Encoding.Unicode;

			TableBorderCharacters.FromJson(@".\Configs\tableBorderCharacters.json");
			CommandTree.FromJsonFolder(@".\Configs\Commands");
			Settings.FromJson(@".\Configs\settings.json");

			var document = new Document();

			InstanceMap.Add(() => document);
			InstanceMap.Add(() => document.GetActiveTable());
			InstanceMap.Add(() => document.GetActiveTable().GetSelectedCells());

			//foreach (var cell in table.Cells)
			//{
			//if (cell.Position.X % 3 == 0)
			//{
			//cell.ForegroundColor = ConsoleColor.Black;
			//cell.BackgroundColor = ConsoleColor.White;
			//}
			//}


			//document.GetActiveTable()[1, 0].SetContent("T");
			//document.GetActiveTable()[1, 0].VertialAlignment = VertialAlignment.Top;
			//document.GetActiveTable()[1, 1].SetContent("B");
			//document.GetActiveTable()[1, 1].VertialAlignment = VertialAlignment.Bottom;
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
					Draw(document.GetActiveTable());


					var rawCommand = Console.ReadLine().Trim();//ReadInput();

					if (rawCommand == "exit")
					{
						break;
					}
					else if (rawCommand.Contains("-help"))
					{
						GetHelp(rawCommand);
					}
					else if (rawCommand == "refresh")
					{
						Console.ResetColor();
						rawCommand = "";
						Console.Clear();
					}
					else
					{
						try
						{
							var command = Command.FromString(rawCommand);

							command.Execute(InstanceMap.GetInstances(command.Reference.ClassName));

							lastHelp = "Enter command to execute";
						}
						catch (IncompleteCommandException)
						{
							GetHelp($"{rawCommand} -help", "Incomplete command");
						}
						catch (TargetParameterCountException)
						{
							GetHelp($"{rawCommand} -help", "Arguments needed");
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"{ex.Message} -> {ex}");
					Console.Write("Press any key to continue");
					Console.ReadKey();
				}

				Console.Clear();
			}
			while (true);

			Console.WriteLine("Good bye!");

			Console.ReadKey();
		}

		public static void GetHelp(string rawCommand, string error = null)
		{
			var command = Command.FromString(rawCommand);

			if (command.AvailableKeys is { })
			{
				lastHelp = $"{error}\n    Available keys:\n        {string.Join("\n        ", command.AvailableKeys)}\n    in '{rawCommand.Replace("-help", "").TrimEnd()}'".Trim();
			}
			else if (command.Reference is { })
			{
				var instances = InstanceMap.GetInstances(command.Reference.ClassName);

				var parameters = command.GetParameters(command.GetMethod(instances.First()));

				lastHelp = $"{error}\n    Parameters:\n        {string.Join("\n        ", parameters)}\n    of '{rawCommand.Replace("-help", "").TrimEnd()}'".Trim();
			}
		}

		public static void Draw(Table table)
		{
			TableRenderer.Render(table);

			if (lastHelp is { })
			{
				Console.Write("Help:\n");

				foreach (var c in lastHelp)
				{
					Console.Write(c);

					Task.Delay(10).Wait();
				}

				Console.WriteLine("\n");
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
