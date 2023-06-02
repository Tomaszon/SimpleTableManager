using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	[CommandInformation("Basic application related commands")]
	public class App
	{
		[CommandReference]
		public void Exit()
		{
			var answer = Shared.ReadLineWhile("Are you sure y/n", new[] { "y", "n" });

			if (answer.ToLower() == "y")
			{
				Console.WriteLine("Good bye!");

				Console.ReadKey();

				Environment.Exit(0);
			}
		}

		[CommandReference()]
		[CommandInformation("Refreshes the view")]
		public void Refresh()
		{
			Console.ResetColor();
			Console.Clear();
		}

		[CommandReference]
		public void SetRenderingMode(RenderingMode renderingMode)
		{
			Renderer.RendererSettings.RenderingMode = renderingMode;
		}
	}
}
