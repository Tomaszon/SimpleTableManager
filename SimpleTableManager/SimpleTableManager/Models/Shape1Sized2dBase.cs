namespace SimpleTableManager.Models;

[ParseFormat("size", "(?<s>\\d+)")]
[method: JsonConstructor]
public abstract class Shape1Sized2dBase<T>(double size1) : ParsableBase<T>, IShape2d, IShape1Sized, IParseCore<T>
where T : Shape1Sized2dBase<T>, IParsable<T>
{
	public double Size1 { get; set; } = size1;

	public abstract double Area { get; }

	public abstract double Perimeter { get; }

	public Shape1Sized2dBase(Shape1Sized2dBase<T> shape) : this(shape.Size1) { }

	public override string ToString()
	{
		return $"S1:{Size1}";
	}

	public static T ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var size1 = double.Parse(args["s"].Value);

		return (T)Activator.CreateInstance(typeof(T), size1)!;
	}
}
