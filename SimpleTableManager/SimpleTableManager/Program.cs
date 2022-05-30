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

		public static InstanceMap InstanceMap { get; set; } = new InstanceMap();


		private static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.Unicode;
			Console.InputEncoding = System.Text.Encoding.Unicode;

			BorderCharacters.FromJson(@".\Configs\borderCharacters.json");
			CommandTree.FromJsonFolder(@".\Configs\Commands");
			Settings.FromJson(@".\Configs\settings.json");
			CellBorders.FromJson(@".\Configs\cellBorders.json");

			var document = new Document();
			var app = new App();

			InstanceMap.Add(() => app);
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


			//document.GetActiveTable().Header[0].AppendRightEllipsis(456);

			//document.GetActiveTable()[0, 0].BorderForegroundColor = ConsoleColor.Yellow;
			//document.GetActiveTable()[1, 0].ForegroundColor = ConsoleColor.Red;
			//document.GetActiveTable()[1, 0].BackgroundColor = ConsoleColor.Cyan;
			//document.GetActiveTable()[0, 1].BorderForegroundColor = ConsoleColor.Yellow;
			//document.GetActiveTable()[1, 1].BorderBackgroundColor = ConsoleColor.White;
			//document.GetActiveTable()[0, 2].BorderForegroundColor = ConsoleColor.Red;
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
				string rawCommand = "";

				try
				{
					Draw(document.GetActiveTable());

					rawCommand = Console.ReadLine().Trim();//ReadInput();

					var command = Command.FromString(rawCommand);

					if (command.Reference.MethodName == Shared.HELP_COMMAND)
					{
						lastHelp = app.ShowHelp(command);
					}
					else
					{
						command.Execute(InstanceMap.GetInstances(command.Reference.ClassName));

						lastHelp = "Enter command to execute";
					}

				}
				catch (Exception ex) when (ex is IncompleteCommandException || ex is TargetParameterCountException)
				{
					lastHelp = app.ShowHelp($"{rawCommand} help", ex.Message);
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

	public class App
	{
		[CommandReference]
		public void Exit()
		{
			var answer = Shared.ReadLineWhile("Are you sure y/n", new[] { "y", "n" });

			if (answer.ToLower() == "y")
			{
				Console.WriteLine("Good bye!");

				Console.ReadKey();

				Environment.Exit(0);
			}
		}

		[CommandReference]
		public void Refresh()
		{
			Console.ResetColor();
			Console.Clear();
		}

		public string ShowHelp(Command command, string error = null)
		{
			if (command.AvailableKeys is { })
			{
				return $"{error}\n    Available keys:\n        {string.Join("\n        ", command.AvailableKeys)}\n    in '{command.RawCommand.Replace(Shared.HELP_COMMAND, "").TrimEnd()}'".Trim();
			}
			else if (command.Reference is { })
			{
				var instances = Program.InstanceMap.GetInstances(command.Reference.ClassName);

				var parameters = command.GetParameters(command.GetMethod(instances.First()));

				return $"{error}\n    Parameters:\n        {string.Join("\n        ", parameters)}\n    of '{command.RawCommand.Replace(Shared.HELP_COMMAND, "").TrimEnd()}'".Trim();
			}

			return "¯\\_(ツ)_/¯";
		}

		public string ShowHelp(string rawCommand, string error = null)
		{
			var command = Command.FromString(rawCommand);

			return ShowHelp(command, error);
		}
	}
}
