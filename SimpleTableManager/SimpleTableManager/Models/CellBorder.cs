using SimpleTableManager.Extensions;

namespace SimpleTableManager.Models
{
	public class CellBorder
	{
		public BorderType Top { get; set; } = BorderType.Horizontal;

		public BorderType Bottom { get; set; } = BorderType.Horizontal;

		public BorderType Left { get; set; } = BorderType.Vertical;

		public BorderType Right { get; set; } = BorderType.Vertical;

		public BorderType TopLeft { get; set; } = BorderType.Right | BorderType.Down;

		public BorderType TopRight { get; set; } = BorderType.Left | BorderType.Down;

		public BorderType BottomLeft { get; set; } = BorderType.Right | BorderType.Up;

		public BorderType BottomRight { get; set; } = BorderType.Left | BorderType.Up;

		public CellBorder Trim(bool top = false, bool bottom = false, bool left = false, bool right = false)
		{
			return ModifySides(BorderType.None, true, top, bottom, left, right);
		}

		public CellBorder Dot(bool top = false, bool bottom = false, bool left = false, bool right = false)
		{
			return ModifySides(BorderType.Dotted, false, top, bottom, left, right);
		}

		public CellBorder ModifySides(BorderType border, bool replace, bool top = false, bool bottom = false, bool left = false, bool right = false)
		{
			CellBorder result = (CellBorder)MemberwiseClone();

			GetType().GetProperties().ForEach(p =>
			{
				if (top && p.Name.Contains("Top") ||
					bottom && p.Name.Contains("Bottom") ||
					left && p.Name.Contains("Left") ||
					right && p.Name.Contains("Right"))
				{
					p.SetValue(result, replace ? border : (BorderType)p.GetValue(this) | border);
				}
			});

			return result;
		}
	}
}
