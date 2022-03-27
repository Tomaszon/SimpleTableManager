namespace SimpleTableManager.Models
{
	public class ViewOptions
	{
		public Position StartPosition { get; set; }

		public Position EndPosition { get; set; }

		public ViewOptions(int x1, int y1, int x2, int y2)
		{
			StartPosition = new Position(x1, y1);
			EndPosition = new Position(x2, y2);
		}
	}
}
