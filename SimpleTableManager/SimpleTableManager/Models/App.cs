using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[CommandInformation("Basic application related commands")]
public class App : CommandExecuterBase
{
	private static readonly string[] _options = new[] { "y", "n" };

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