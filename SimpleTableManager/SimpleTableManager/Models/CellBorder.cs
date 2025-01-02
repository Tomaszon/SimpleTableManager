namespace SimpleTableManager.Models;

[method: JsonConstructor]
public struct CellBorder()
{
	public BorderType Top { get; set; } = BorderType.Horizontal;

	public BorderType Bottom { get; set; } = BorderType.Horizontal;

	public BorderType Left { get; set; } = BorderType.Vertical;

	public BorderType Right { get; set; } = BorderType.Vertical;

	public BorderType TopLeft { get; set; } = BorderType.Right | BorderType.Down;

	public BorderType TopRight { get; set; } = BorderType.Left | BorderType.Down;

	public BorderType BottomLeft { get; set; } = BorderType.Right | BorderType.Up;

	public BorderType BottomRight { get; set; } = BorderType.Left | BorderType.Up;

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