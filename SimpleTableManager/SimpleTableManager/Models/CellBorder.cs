namespace SimpleTableManager.Models;

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

	public CellBorder TrimCorner(bool topLeft = false, bool topRight = false, bool bottomLeft = false, bool bottomRight = false)
	{
		return ModifyCorners(BorderType.None, true, topLeft, topRight, bottomLeft, bottomRight);
	}

	public CellBorder TrimSide(bool top = false, bool bottom = false, bool left = false, bool right = false)
	{
		return ModifySides(BorderType.None, true, top, bottom, left, right);
	}

	public CellBorder Dot(bool top = false, bool bottom = false, bool left = false, bool right = false)
	{
		return ModifySides(BorderType.Dotted, false, top, bottom, left, right);
	}

	public CellBorder ModifySides(BorderType border, bool replace, bool top = false, bool bottom = false, bool left = false, bool right = false)
	{
		var result = (CellBorder)MemberwiseClone();

		GetType().GetProperties().ForEach(p =>
		{
			if (top && p.Name.Contains("Top") ||
				bottom && p.Name.Contains("Bottom") ||
				left && p.Name.Contains("Left") ||
				right && p.Name.Contains("Right"))
			{
				p.SetValue(result, replace ? border : (BorderType)p.GetValue(this)! | border);
			}
		});

		return result;
	}

	public CellBorder ModifyCorners(BorderType border, bool replace, bool topLeft, bool topRight, bool bottomLeft, bool bottomRight)
	{
		var result = (CellBorder)MemberwiseClone();

		if (topLeft)
		{
			result.TopLeft = replace ? border : result.TopLeft | border;
		}

		if (topRight)
		{
			result.TopRight = replace ? border : result.TopRight | border;
		}

		if (bottomLeft)
		{
			result.BottomLeft = replace ? border : result.BottomLeft | border;
		}

		if (bottomRight)
		{
			result.BottomRight = replace ? border : result.BottomRight | border;
		}

		return result;
	}
}