namespace SimpleTableManager.Models
{
	public class ViewOptions
	{
		public Size Size { get; set; }

		public Position Position { get; set; }

		public ViewOptions(int x, int y, int w, int h)
		{
			Position = new Position(x, y);
			Size = new Size(w, h);
		}
	}
}
