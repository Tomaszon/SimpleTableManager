namespace SimpleTableManager.Models;

public class ContentPadding(int top = 0, int bottom = 0, int left = 0, int right = 0)
{
	public int Top { get; set; } = top;

	public int Bottom { get; set; } = bottom;

	public int Left { get; set; } = left;

	public int Right { get; set; } = right;

	public override string ToString()
	{
		return $"T:{Top}, B:{Bottom}, L:{Left}, R:{Right}";
	}
}