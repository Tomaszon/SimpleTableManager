namespace SimpleTableManager.Models;

public readonly struct ContentPadding(int top = 0, int bottom = 0, int left = 0, int right = 0)
{
	public int Top { get;} = top;

	public int Bottom { get; } = bottom;

	public int Left { get; } = left;

	public int Right { get; } = right;

	public override readonly string ToString()
	{
		return $"T:{Top}, B:{Bottom}, L:{Left}, R:{Right}";
	}
}