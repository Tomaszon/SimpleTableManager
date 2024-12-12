using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

[ParseFormat("size", "(?<s>\\d)")]
public abstract class Shape1Sized2dBase<T> : ParsableBase<T>, IShape2d, IShape1Sized, IParseCore<T>
where T : Shape1Sized2dBase<T>, IParsable<T>, new()
{
	public decimal Size1 { get; set; }

	public abstract decimal Area { get; }

	public abstract decimal Perimeter { get; }

	public Shape1Sized2dBase() { }

	[JsonConstructor]
	public Shape1Sized2dBase(decimal size1)
	{
		Size1 = size1;
	}

	public Shape1Sized2dBase(Shape1Sized2dBase<T> shape)
	{
		Size1 = shape.Size1;
	}

	public override string ToString()
	{
		return $"S1:{Size1}";
	}

	public static T ParseCore(GroupCollection args)
	{
		var size1 = decimal.Parse(args["s"].Value);

		return new T() { Size1 = size1 };
	}
}
