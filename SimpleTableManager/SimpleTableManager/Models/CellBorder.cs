namespace SimpleTableManager.Models
{
	public class CellBorder
	{
		public BorderType Top { get; set; } = BorderType.Horizontal;

		public BorderType Bottom { get; set; } = BorderType.Horizontal;

		public BorderType Left { get; set; } = BorderType.Vertical;

		public BorderType Right { get; set; } = BorderType.Vertical;
	}
}
