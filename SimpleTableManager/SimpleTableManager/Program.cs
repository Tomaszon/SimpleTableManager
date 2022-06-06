using System;

using SimpleTableManager.Models;
using SimpleTableManager.Services;

namespace SimpleTableManager
{
	internal class Program
	{
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
				try
				{
					SmartConsole.Draw(document);

					//var rawCommand = Console.ReadLine().Trim();
					var rawCommand = SmartConsole.ReadInput();

					var command = Command.FromString(rawCommand);

					command.Execute(InstanceMap.GetInstances(command.Reference.ClassName));

					SmartConsole.LastHelp = "Enter command to execute";
				}
				catch (ParameterCountException ex)
				{
					SmartConsole.ShowHelp(ex.RawCommand, null, ex.CommandReference, ex.Message);
				}
				catch (IncompleteCommandException ex)
				{
					SmartConsole.ShowHelp(ex.RawCommand, ex.AvailableKeys, null, ex.Message);
				}
				catch (PartialKeyException ex)
				{
					SmartConsole.ShowHelp(ex.RawCommand, null, null, ex.Message);
				}
				catch (HelpRequestedException ex)
				{
					SmartConsole.ShowHelp(ex.RawCommand, ex.AvailableKeys, ex.CommandReference, ex.Message);
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
	}
}
