namespace SimpleTableManager.Models
{
	public class ContentAlignment
	{
		public HorizontalAlignment Horizontal { get; set; }

		public VerticalAlignment Vertical { get; set; }

		public ContentAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
		{
			Horizontal = horizontal;
			Vertical = vertical;
		}

		public static implicit operator ContentAlignment((HorizontalAlignment horizontal, VerticalAlignment vertical) record)
		{
			return new ContentAlignment(record.horizontal, record.vertical);
		}

		public override string ToString()
		{
			return $"H: {Horizontal}, V: {Vertical}";
		}
	}
}
