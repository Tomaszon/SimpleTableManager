using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SimpleTableManager.Models;
using SimpleTableManager.Models.Exceptions;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager
{
	internal class Program
	{
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

			InstanceMap.Instance.Add(() => app);
			InstanceMap.Instance.Add(() => document);
			InstanceMap.Instance.Add(() => document.GetActiveTable());
			InstanceMap.Instance.Add(() => document.GetActiveTable().GetSelectedCells());

			#region test
			// var function = FunctionCollection.GetFunction(NumericFunctionOperator.Sum, new[] { new FunctionParameter(5), new FunctionParameter(4) });

			// var result = function.Execute();

			document.GetActiveTable()[1, 0].ContentType = typeof(decimal);
			document.GetActiveTable()[1, 0].SetContent(5, 2);

			document.GetActiveTable()[2, 0].ContentType = typeof(decimal);
			document.GetActiveTable()[2, 0].SetContent(4);

			var cell11 = document.GetActiveTable()[1,1];
			cell11.SetContentColor(ConsoleColor.Red, ConsoleColor.Yellow);
			cell11.SetBorderColor(ConsoleColor.Red, ConsoleColor.Yellow);

			var cell31 = document.GetActiveTable()[3, 0];
			cell31.SetContent(NumericFunctionOperator.Avg, "1,0", "2,0");
			cell31.SetHorizontalAlignment(HorizontalAlignment.Left);
			cell31.SetBorderColor(ConsoleColor.Red);

			var cell42 = document.GetActiveTable()[4, 1];
			cell42.SetBorderColor(ConsoleColor.Green);

			var cell32 = document.GetActiveTable()[3, 1];
			cell32.SetBorderColor(ConsoleColor.Magenta);

			document.GetActiveTable()[4, 0].SetContent(ObjectFunctionOperator.Const, "szilva");

			document.GetActiveTable()[5, 0].ContentType = typeof(decimal);
			document.GetActiveTable()[5, 0].ContentFunction = FunctionCollection.GetFunction(StringFunctionOperator.Len, new[]
			{
				new ObjectFunction(new Position(4, 0))
			});

			document.GetActiveTable()[6, 0].SetContent("const", "alma", "körte");

			document.GetActiveTable()[1, 1].SetContent("con", "4,0", "6,0");

			document.GetActiveTable()[2, 1].SetContent("join", "1,0", "4,0");

			document.GetActiveTable()[3, 1].ContentFunction = FunctionCollection.GetFunction(StringFunctionOperator.Join, new[]
			{
				FunctionCollection.GetFunction(NumericFunctionOperator.Avg, new[]
				{
					new ObjectFunction(new Position(2, 0)),
				 	new ObjectFunction(new Position(5, 0))
				}),
				FunctionCollection.GetFunction(StringFunctionOperator.Len, new[]
				{
					new ObjectFunction(new Position(4, 0))
				})
			});

			document.GetActiveTable()[4, 1].SetContent("sum", "1,0", 4);


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
			#endregion test
			do
			{
				try
				{
					SmartConsole.Render(document);

					var rawCommand = SmartConsole.ReadInputString();

					var command = Command.FromString(rawCommand);

					var results = command.Execute(InstanceMap.Instance.GetInstances(command.Reference.ClassName, out _));

					SmartConsole.ShowResults(results);
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


		//	table set content avg 3 4
		//	table set content sum(avg(3 5 1,1) 9 2,1 avg(3,1 9))
		//	
	}
}
