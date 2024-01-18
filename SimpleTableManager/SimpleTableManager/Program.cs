using SimpleTableManager.Models;
using SimpleTableManager.Services;

namespace SimpleTableManager;

public class Program
{
	private static void Main()
	{
		Console.OutputEncoding = System.Text.Encoding.Unicode;
		Console.InputEncoding = System.Text.Encoding.Unicode;

		BorderCharacters.FromJson(@".\Configs\borderCharacters.json");
		CommandTree.FromJsonFolder(@".\Configs\Commands");
		Settings.FromJson(@".\Configs\settings.json");
		CellBorders.FromJson(@".\Configs\cellBorders.json");
		CommandShortcuts.FromJson(@".\Configs\commandShortcuts.json");

		var app = new App(new Document(Settings.Current.DefaultTableSize));

		InstanceMap.Instance.Add(() => app);
		InstanceMap.Instance.Add(() => app.Document);
		InstanceMap.Instance.Add(() => app.Document.GetActiveTable());
		InstanceMap.Instance.Add(() => app.Document.GetActiveTable().GetSelectedCells());

		#region test

		// var function = FunctionCollection.GetFunction(NumericFunctionOperator.Sum, new[] { new FunctionParameter(5), new FunctionParameter(4) });

		// var result = function.Execute();

		// IFunction2 fn = new StringFunction2()
		// {
		// 	Operator = StringFunctionOperator.Const,
		// 	NamedArguments = new Dictionary<string, string>() { { "separator", "," } },
		// 	Arguments = new[] { "al,ma", "körte" }
		// };
		// object result = fn.Execute(out _).ToList();

		// fn = new DecimalNumericFunction2()
		// {
		// 	Operator = NumericFunctionOperator.Ceiling,
		// 	NamedArguments = new Dictionary<string, string> { { "decimals", "0" } },
		// 	Arguments = new[] { 5.5m, 2.2m, 3.9m }
		// };
		// result = fn.Execute(out _).ToList();

		// Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented));
		// document.GetActiveTable()[0, 0].IsSelected = true;
		// document.GetActiveTable()[0,0].IncreaseLayerIndex();
		// app.SetRenderingMode(RenderingMode.Layer);

		//document.GetActiveTable()[1,1].SetContent2("string", "con", "alma", "körte", "szilva");

		// result = document.GetActiveTable()[1,1].ContentFunction2;

		// Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented));

		// var cell11 = document.GetActiveTable()[1, 1];
		// cell11.SetContentColor(ConsoleColor.Red, ConsoleColor.Yellow);
		// cell11.SetBorderColor(ConsoleColor.Red, ConsoleColor.Yellow);

		// var cell31 = document.GetActiveTable()[3, 0];
		// cell31.SetContent(NumericFunctionOperator.Avg, "1,0", "2,0");
		// cell31.SetHorizontalAlignment(HorizontalAlignment.Left);
		// cell31.SetBorderColor(ConsoleColor.Red);

		// var cell42 = document.GetActiveTable()[4, 1];
		// cell42.SetBorderColor(ConsoleColor.Green);

		// var cell32 = document.GetActiveTable()[3, 1];
		// cell32.SetBorderColor(ConsoleColor.Magenta);

		// document.GetActiveTable()[4, 0].SetContent(ObjectFunctionOperator.Const, "szilva");

		// document.GetActiveTable()[6, 0].SetContent("const", "alma", "körte");

		// document.GetActiveTable()[1, 1].SetContent("con", "4,0", "6,0");

		// document.GetActiveTable()[2, 1].SetContent("join", "1,0", "4,0");

		// document.GetActiveTable()[3, 1].ContentFunction = FunctionCollection.GetFunction(StringFunctionOperator.Join, new[]
		// {
		// 	FunctionCollection.GetFunction(NumericFunctionOperator.Avg, new[]
		// 	{
		// 		new ObjectFunction(new Position(2, 0)),
		// 	 	new ObjectFunction(new Position(5, 0))
		// 	}),
		// 	FunctionCollection.GetFunction(StringFunctionOperator.Len, new[]
		// 	{
		// 		new ObjectFunction(new Position(4, 0))
		// 	})
		// });

		// document.GetActiveTable()[4, 1].SetContent("sum", "1,0", 4);


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
				SmartConsole.Render(app.Document);

				SmartConsole.Play(Note.Ok);

				var rawCommand = SmartConsole.ReadInput(out var command);

				command ??= Command.FromString(rawCommand);

				var results = command.Execute(InstanceMap.Instance.GetInstances(command.Reference!.ClassName, out var type), type);

				SmartConsole.ShowResults(results);

				SmartConsole.Play(Note.Ok);
			}
			catch (ArgumentCountException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, null, ex.CommandReference, ex.Message);

				SmartConsole.Play(Note.Error);
			}
			catch (IncompleteCommandException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, ex.AvailableKeys, null, ex.Message);

				SmartConsole.Play(Note.Error);
			}
			catch (PartialKeyException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, null, null, ex.Message);

				SmartConsole.Play(Note.Question);
			}
			catch (HelpRequestedException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, ex.AvailableKeys, ex.CommandReference, ex.Message);

				SmartConsole.Play(Note.Question);
			}
			catch (Exception ex) when (ex.IsHandled())
			{
				Console.WriteLine(ex.Message);
				Console.Write("Press any key to continue");

				SmartConsole.Play(Note.Error);

				Console.ReadKey();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex.Message} -> {ex}");
				Console.Write("Press any key to continue");

				SmartConsole.Play(Note.Critical);

				Console.ReadKey();
			}
		}
		while (true);
	}
	//	table set content avg 3 4
	//	table set content sum(avg(3 5 1,1) 9 2,1 avg(3,1 9))
}