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

		OnStateModifierCommandExecuted();
	}

	[CommandFunction]
	public void Reconfig()
	{
		BorderCharacters.FromJson(@".\Configs\borderCharacters.json");
		CommandTree.FromJsonFolder(@".\Configs\Commands");
		Settings.FromJson(@".\Configs\settings.json");
		CellBorders.FromJson(@".\Configs\cellBorders.json");
		CommandShortcuts.FromJson(@".\Configs\commandShortcuts.json");
		Localizer.FromJson(@".\Configs\Localizations");

		CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(Settings.Current.Culture);
	}

	public override void OnStateModifierCommandExecuted()
	{
		using var m = new MemoryStream();
		using var sw = new StreamWriter(m);
		using var sr = new StreamReader(m);

		Document.Serialize(sw);

		sw.Flush();

		m.Position = 0;

		var content = sr.ReadToEnd();

		EditHistory.Add(content);
	}

	[CommandFunction, CommandShortcut("redoChange")]
	public void Redo()
	{
		if (EditHistory.TryGetNextHistoryItem(out var state))
		{
			using var m = new MemoryStream();
			using var sw = new StreamWriter(m);
			using var sr = new StreamReader(m);

			sw.Write(state);

			sw.Flush();

			m.Position = 0;

			Document.Deserialize(sr);
		}
	}

	[CommandFunction, CommandShortcut("undoChange")]
	public void Undo()
	{
		if (EditHistory.TryGetPreviousHistoryItem(out var state))
		{
			using var m = new MemoryStream();
			using var sw = new StreamWriter(m);
			using var sr = new StreamReader(m);

			sw.Write(state);

			sw.Flush();

			m.Position = 0;

			Document.Deserialize(sr);
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