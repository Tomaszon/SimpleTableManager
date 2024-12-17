using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

[ParseFormat("size1,size2", "(?<s>\\d+),(?<s2>\\d+)"), ParseFormat("size1;size2", "(?<s>\\d+);(?<s2>\\d+)")]
public abstract class Shape2Sized2dBase<T> : Shape1Sized2dBase<T>, IShape2Sized, IParseCore<T>
where T : Shape2Sized2dBase<T>, IParsable<T>, new()
{
	public double Size2 { get; set; }

	public Shape2Sized2dBase() { }

	public Shape2Sized2dBase(double size1, double size2) : base(size1)
	{
		Size2 = size2;
	}

	public Shape2Sized2dBase(Shape2Sized2dBase<T> shape) : base(shape)
	{
		Size2 = shape.Size2;
	}

	public override string ToString()
	{
		return $"S1:{Size1}, S2:{Size2}";
	}

	public new static T ParseCore(GroupCollection args)
	{
		var size1 = double.Parse(args["s"].Value);
		var size2 = args["s2"].Success ? double.Parse(args["s2"].Value) : size1;

		return new T() { Size1 = size1, Size2 = size2 };
	}
}
