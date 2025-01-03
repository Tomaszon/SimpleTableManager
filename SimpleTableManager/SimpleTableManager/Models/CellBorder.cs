namespace SimpleTableManager.Models;

[method: JsonConstructor]
public struct CellBorder()
{
	public BorderType Top { get; set; }

	public BorderType Bottom { get; set; }

	public BorderType Left { get; set; }

	public BorderType Right { get; set; }

	public BorderType TopLeft { get; set; }

	public BorderType TopRight { get; set; }

	public BorderType BottomLeft { get; set; }

	public BorderType BottomRight { get; set; }

	public readonly CellBorder TrimCorner(bool topLeft = false, bool topRight = false, bool bottomLeft = false, bool bottomRight = false)
	{
		var instance = this;

		if (topLeft)
		{
			instance.TopLeft = BorderType.None;
		}

		if (topRight)
		{
			instance.TopRight = BorderType.None;
		}

		if (bottomLeft)
		{
			instance.BottomLeft = BorderType.None;
		}

		if (bottomRight)
		{
			instance.BottomRight = BorderType.None;
		}

		return instance;
	}

	public readonly CellBorder TrimSide(bool top = false, bool bottom = false, bool left = false, bool right = false)
	{
		var instance = this;

		if (top)
		{
			instance.TopLeft = BorderType.None;
			instance.Top = BorderType.None;
			instance.TopRight = BorderType.None;
		}

		if (bottom)
		{
			instance.BottomLeft = BorderType.None;
			instance.Bottom = BorderType.None;
			instance.BottomRight = BorderType.None;
		}

		if (left)
		{
			instance.TopLeft = BorderType.None;
			instance.Left = BorderType.None;
			instance.BottomLeft = BorderType.None;
		}

		if (right)
		{
			instance.TopRight = BorderType.None;
			instance.Right = BorderType.None;
			instance.BottomRight = BorderType.None;
		}

		return instance;
	}
}