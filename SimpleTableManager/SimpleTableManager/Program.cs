namespace SimpleTableManager;

[ExcludeFromCodeCoverage]
public static class Program
{
	private static void Main(params string[] args)
	{
		Console.TreatControlCAsInput = true;
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		Console.InputEncoding = System.Text.Encoding.UTF8;

		Settings.FromJson(@"Configs/settings.json");

		var app = new App(new Document(Settings.Current.DefaultTableSize));

		app.Reconfig();

		InstanceMap.Instance.Add(() => app);
		InstanceMap.Instance.Add(() => app.Document);
		InstanceMap.Instance.Add(() => app.Document.GetActiveTable());
		InstanceMap.Instance.Add(() => app.Document.GetActiveTable().GetPrimarySelectedCells());

		SmartConsole.PlayAsync(Settings.Current.StartupNotes);

		Renderer.RenderLoadingScreen();

		do
		{
			try
			{
				if (args.Length > 0)
				{
					var fileName = args[0];

					args = [];

					InstanceMap.Instance.GetInstance<Document>()!.Open(fileName);
				}

				SmartConsole.Render(app.Document);

				var rawCommand = SmartConsole.ReadInput(out var command);

				command ??= Command.FromString(rawCommand);

				var results = command.Execute(InstanceMap.Instance.GetInstances(command.Reference!.Value.ClassName, out var type), type);

				SmartConsole.ShowResults(results);

				SmartConsole.PlayAsync(Settings.Current.OkNotes);
			}
			catch (ArgumentCountException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, null, ex.CommandReference, ex.Message);

				SmartConsole.PlayAsync(Settings.Current.ErrorNotes);
			}
			catch (IncompleteCommandException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, ex.AvailableKeys, null, ex.Message);

				SmartConsole.PlayAsync(Settings.Current.ErrorNotes);
			}
			catch (PartialKeyException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, null, null, ex.Message);

				SmartConsole.PlayAsync(Settings.Current.QuestionNotes);
			}
			catch (HelpRequestedException ex)
			{
				SmartConsole.ShowHelp(ex.RawCommand, ex.AvailableKeys?.Select(k => k.key).ToList(), ex.CommandReference, ex.Message);

				SmartConsole.PlayAsync(Settings.Current.QuestionNotes);
			}
			catch (LocalizationException ex)
			{
				Renderer.ChangeToNotOkLabelColors();

				Console.Write("Translation not found:\n");

				Renderer.ChangeToTextColors();

				Console.WriteLine($"   {ex.Message}");

				Console.Write("Press any key to continue");

				SmartConsole.PlayAsync(Settings.Current.CriticalNotes);

				Console.ReadKey();
			}
			catch (Exception ex) when (ex.IsHandled())
			{
				Renderer.ChangeToNotOkLabelColors();

				Console.WriteLine("Error\n");

				Renderer.ChangeToTextColors();

				Console.WriteLine($"{ex.Message}{(ex.InnerException is not null ? $" -> {ex.InnerException.Message}" : "")}");
				Console.Write("Press any key to continue");

				SmartConsole.PlayAsync(Settings.Current.ErrorNotes);

				Console.ReadKey();
			}
			catch (Exception ex)
			{
				Renderer.ChangeToNotOkLabelColors();

				Console.WriteLine("Unexpected error\n");

				Renderer.ChangeToTextColors();

				Console.Write($"{ex.Message} -> \n\n");

				Renderer.ChangeToNotOkLabelColors();

				Console.Write("Details:\n");

				Renderer.ChangeToTextColors();

				Console.WriteLine($"   {ex}");

				Console.Write("Press any key to continue");

				SmartConsole.PlayAsync(Settings.Current.CriticalNotes);

				Console.ReadKey();
			}
		}
		while (true);
	}
}