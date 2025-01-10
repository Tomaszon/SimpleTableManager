namespace SimpleTableManager.Models;

public struct ContentAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
{
	public HorizontalAlignment Horizontal { get; set; } = horizontal;

	public VerticalAlignment Vertical { get; set; } = vertical;

	public override readonly string ToString()
	{
		return $"H:{Horizontal}, V:{Vertical}";
	}
}