using System.Globalization;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Basic application related commands")]
public class App : CommandExecuterBase
{
	public HistoryList EditHistory { get; set; } = new(Settings.Current.EditHistoryLength, true, -1);

	public Document Document { get; set; }

	private static readonly string[] _options = ["y", "n"];

	public App(Document document)
	{
		Document = document;

		document.StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted;

		EditHistory.Init(Shared.SerializeObject(Document));
	}

	[CommandFunction]
	public void Reconfig()
	{
		Settings.FromJson(@"Configs/settings.json");
		CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(Settings.Current.Culture);

		Localizer.FromJson(@"Configs/Localizations");
		BorderCharacters.FromJson(@"Configs/borderCharacters.json");
		CommandTree.FromJsonFolder(@"Configs/Commands");
		CellBorders.FromJson(@"Configs/cellBorders.json");
		CommandShortcuts.FromJson(@"Configs/commandShortcuts.json");
	}

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, StateModifierCommandExecutedEventArgs args)
	{
		EditHistory.Add(Shared.SerializeObject(Document));
	}

	[CommandFunction, CommandShortcut("redoChange")]
	public void Redo()
	{
		if (EditHistory.TryGetNextHistoryItem(out var state))
		{
			using var ms = new MemoryStream();
			using var sw = new StreamWriter(ms);
			using var sr = new StreamReader(ms);

			sw.Write(state);

			sw.Flush();

			ms.Position = 0;

			Shared.PopulateDocument(sr, Document);

			sr.Close();
			sw.Close();
			ms.Close();
		}
	}

	[CommandFunction, CommandShortcut("undoChange")]
	public void Undo()
	{
		if (EditHistory.TryGetPreviousHistoryItem(out var state))
		{
			using var ms = new MemoryStream();
			using var sw = new StreamWriter(ms);
			using var sr = new StreamReader(ms);

			sw.Write(state);

			sw.Flush();

			ms.Position = 0;

			Shared.PopulateDocument(sr, Document);

			sr.Close();
			sw.Close();
			ms.Close();
		}
	}

	[CommandFunction(StateModifier = false)]
	public static void SetAutoSave(bool autoSave)
	{
		Settings.Current.AutoSave = autoSave;
	}

	[CommandFunction(StateModifier = false)]
	public static void Exit()
	{
		var answer = SmartConsole.ReadLineWhile("Are you sure y/n", _options);

		if (answer.ToLower().Equals("y"))
		{
			Console.WriteLine("Good bye!");

			SmartConsole.PlayAsync(Settings.Current.ShutdownNotes);

			Console.ReadKey();

			Environment.Exit(0);
		}
	}

	[CommandFunction(StateModifier = false), CommandShortcut, CommandInformation("Refreshes the view")]
	public static void Refresh()
	{
		Console.ResetColor();
		Console.Clear();
	}

	[CommandFunction(StateModifier = false)]
	public static void SetRenderingMode(RenderingMode renderingMode)
	{
		Renderer.RendererSettings.RenderingMode = renderingMode;
	}
}