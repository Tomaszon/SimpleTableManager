using System;

using SimpleTableManager.Models;
using SimpleTableManager.Services;

namespace SimpleTableManager
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			TableBorderCharacters.FromJson($@".\Configs\tableBorderCharacters.json");
			CommandTree.FromJson($@".\Configs\commands.json");

			Settings.ModernTableBorder = false;

			ViewOptions viewOptions = null; // new ViewOptions(2, 1, 4, 4);

			var table = new Table("", 9, 9);

			string lastHelp = null;

			foreach (var cell in table.Cells)
			{
				if (cell.Position.X % 3 == 0)
				{
					cell.ForegroundColor = ConsoleColor.Black;
					cell.BackgroundColor = ConsoleColor.White;
				}
			}

			//Console.WriteLine(JsonConvert.SerializeObject(viewOptions));

			do
			{
				try
				{
					TableRenderer.Render(table, viewOptions);

					if (lastHelp is { })
					{
						Console.WriteLine($"Help: {lastHelp}");
					}

					Console.Write("> ");

					var rawCommand = Console.ReadLine();

					if (rawCommand == "exit")
					{
						break;
					}
					else if (rawCommand.Contains("-help"))
					{
						var command = Command.FromString(rawCommand);

						if (command.AvailableKeys is { })
						{
							lastHelp = $"Available keys: '{string.Join(", ", command.AvailableKeys)}' in '{rawCommand.Replace("-help", "").TrimEnd()}'";
						}
						else if (command.Reference is { })
						{
							var parameters = command.GetParameters(command.GetMethod(table));

							lastHelp = $"Parameters: '{string.Join(", ", parameters)}' of '{rawCommand.Replace("-help", "").TrimEnd()}'";
						}
					}
					else if (rawCommand == "refresh")
					{
						continue;
					}
					else
					{
						var command = Command.FromString(rawCommand);
						//var command = Command.FromString("table row add after 5");

						command.Execute(table);
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
	}
}
