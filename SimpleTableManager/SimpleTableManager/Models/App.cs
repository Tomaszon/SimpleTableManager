using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[CommandInformation("Basic application related commands")]
public class App : CommandExecuterBase
{
	public HistoryList EditHistory { get; set; } = new(Settings.Current.EditHistoryLength, -1);

	public Document Document { get; set; }

	private static readonly string[] _options = new[] { "y", "n" };

	public App(Document document)
	{
		Document = document;

		document.StateModifierCommandExecuted += OnStateModifierCommandExecuted;

		OnStateModifierCommandExecuted();
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

	[CommandReference]
	[CommandShortcut("redoChange")]
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

	[CommandReference]
	[CommandShortcut("undoChange")]
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

	[CommandReference(StateModifier = false)]
	public static void SetAutosave(bool autosave)
	{
		Settings.Current.Autosave = autosave;
	}

	[CommandReference(StateModifier = false)]
	public static void Exit()
	{
		var answer = SmartConsole.ReadLineWhile("Are you sure y/n", _options);

		if (answer.ToLower().Equals("y"))
		{
			Console.WriteLine("Good bye!");

			Console.ReadKey();

			Environment.Exit(0);
		}
	}

	[CommandReference(StateModifier = false)]
	[CommandInformation("Refreshes the view")]
	[CommandShortcut("refresh")]
	public static void Refresh()
	{
		Console.ResetColor();
		Console.Clear();
	}

	[CommandReference(StateModifier = false)]
	public static void SetRenderingMode(RenderingMode renderingMode)
	{
		Renderer.RendererSettings.RenderingMode = renderingMode;
	}
}