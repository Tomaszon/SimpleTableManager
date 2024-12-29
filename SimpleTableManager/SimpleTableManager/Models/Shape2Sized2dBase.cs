namespace SimpleTableManager.Models;

[ParseFormat("size1,size2", "(?<s>\\d+),(?<s2>\\d+)"), ParseFormat("size1;size2", "(?<s>\\d+);(?<s2>\\d+)")]
[method: JsonConstructor]
public abstract class Shape2Sized2dBase<T>(double size1, double size2) : Shape1Sized2dBase<T>(size1), IShape2Sized, IParseCore<T>
where T : Shape2Sized2dBase<T>, IParsable<T>
{
	public double Size2 { get; set; } = size2;

	public Shape2Sized2dBase(Shape2Sized2dBase<T> shape) : this(shape.Size1, shape.Size2) { }

	public override string ToString()
	{
		return $"S1:{Size1}, S2:{Size2}";
	}

	public new static T ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var size1 = double.Parse(args["s"].Value);
		var size2 = args["s2"].Success ? double.Parse(args["s2"].Value) : size1;

		return (T)Activator.CreateInstance(typeof(T), size1, size2)!;
	}
}
