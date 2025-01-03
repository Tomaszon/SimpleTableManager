namespace SimpleTableManager.Models;

public readonly struct ContentAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
{
	public HorizontalAlignment Horizontal { get; } = horizontal;

	public VerticalAlignment Vertical { get; } = vertical;

	public static implicit operator ContentAlignment((HorizontalAlignment horizontal, VerticalAlignment vertical) record)
	{
		return new ContentAlignment(record.horizontal, record.vertical);
	}

	public override readonly string ToString()
	{
		return $"H:{Horizontal}, V:{Vertical}";
	}
}