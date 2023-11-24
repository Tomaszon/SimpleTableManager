namespace SimpleTableManager.Models;

public class ContentPadding
{
	public int Top { get; set; } = 0;

	public int Bottom { get; set; } = 0;

	public int Left { get; set; } = 0;

	public int Right { get; set; } = 0;

	public ContentPadding()
	{

	}

	public ContentPadding(int top, int bottom, int left, int right)
	{
		Top = top;
		Bottom = bottom;
		Left = left;
		Right = right;
	}

	public override string ToString()
	{
		return $"T: {Top}, B: {Bottom}, L: {Left}, R: {Right}";
	}
}