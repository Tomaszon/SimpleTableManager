namespace SimpleTableManager.Models;

public class ContentAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
{
	public HorizontalAlignment Horizontal { get; set; } = horizontal;

	public VerticalAlignment Vertical { get; set; } = vertical;

	public static implicit operator ContentAlignment((HorizontalAlignment horizontal, VerticalAlignment vertical) record)
	{
		return new ContentAlignment(record.horizontal, record.vertical);
	}

	public override string ToString()
	{
		return $"H:{Horizontal}, V:{Vertical}";
	}
}