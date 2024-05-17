namespace SimpleTableManager.Models;

[ParseFormat("size", "\\d"), ParseFormat("size1,size2", "\\d,\\d"), ParseFormat("size1;size2", "\\d;\\d")]
public class Shape : ParsableBase<Shape>, IParsable<Shape>
{
	public decimal Size1 { get; set; }

	public decimal? Size2 { get; set; }

	[JsonConstructor]
	public Shape(decimal size1, decimal? size2)
	{
		Size1 = size1;
		Size2 = size2;
	}

	public Shape(Shape shape)
	{
		Size1 = shape.Size1;
		Size2 = shape.Size2;
	}

	public override string ToString()
	{
		return $"Size1: {Size1}, Size2: {Size2}";
	}

	public static Shape Parse(string value, IFormatProvider? provider)
	{
		return ParseWrapper(value, args =>
		{
			var size1 = decimal.Parse(args[0].Trim());
			var size2 = args.Length > 1 ? decimal.Parse(args[1].Trim()) : (decimal?)null;

			return new Shape(size1, size2);
		});
	}
}
