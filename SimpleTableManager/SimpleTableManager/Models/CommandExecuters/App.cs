using System.Globalization;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Basic application related commands")]
public class App : CommandExecuterBase
{
	public HistoryList EditHistory { get; set; } = new(Settings.Current.EditHistoryLength, true, -1);

	public Document Document { get; set; }

	private static readonly string[] _options = new[] { "y", "n" };

	public App(Document document)
	{
		Document = document;

		document.StateModifierCommandExecuted += OnStateModifierCommandExecuted;

		InvokeStateModifierCommandExecutedEvent(this);
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

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, IStateModifierCommandExecuter root)
	{
		using var ms = new MemoryStream();
		using var sw = new StreamWriter(ms);
		using var sr = new StreamReader(ms);

		Document.Serialize(sw);

		sw.Flush();

		ms.Position = 0;

		var content = sr.ReadToEnd();

		EditHistory.Add(content);

		sr.Close();
		sw.Close();
		ms.Close();
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

			Document.Deserialize(sr);

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

			Document.Deserialize(sr);

			sr.Close();
			sw.Close();
			ms.Close();
		}
	}

	[CommandFunction(StateModifier = false)]
	public static void SetAutosave(bool autosave)
	{
		Settings.Current.Autosave = autosave;
	}

	[CommandFunction(StateModifier = false)]
	public static void Exit()
	{
		var answer = SmartConsole.ReadLineWhile("Are you sure y/n", _options);

		if (answer.ToLower().Equals("y"))
		{
			Console.WriteLine("Good bye!");

			SmartConsole.PlayShutdown();

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