using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[CommandInformation("Basic application related commands")]
public class App
{
	private static readonly string[] _options = new[] { "y", "n" };

	[CommandReference]
	public static void Exit()
	{
		var answer = Shared.ReadLineWhile("Are you sure y/n", _options);

		if (answer.ToLower().Equals("y"))
		{
			Console.WriteLine("Good bye!");

			Console.ReadKey();

			Environment.Exit(0);
		}
	}

	[CommandReference]
	[CommandInformation("Refreshes the view")]
	public static void Refresh()
	{
		Console.ResetColor();
		Console.Clear();
	}

	[CommandReference]
	public static void SetRenderingMode(RenderingMode renderingMode)
	{
		Renderer.RendererSettings.RenderingMode = renderingMode;
	}
}