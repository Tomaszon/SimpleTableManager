using System.Globalization;

namespace SimpleTableManager.Models;

[ParseFormat("[size]", "^\\[(?<s>\\d+)\\]$")]
[method: JsonConstructor]
public abstract class Shape1Sized2dBase<T>(double size1) :
	ConvertibleBase<T>,
	IShape1Sized,
	IShape2d,
	IParsableCore<T>
	where T : Shape1Sized2dBase<T>, IParsable<T>
{
	public double Size1 { get; set; } = size1;

	public abstract double Area { get; }

	public abstract double Perimeter { get; }

	public Shape1Sized2dBase(Shape1Sized2dBase<T> shape) :
		this(shape.Size1)
	{ }

	public virtual string ToString(string? format, IFormatProvider? formatProvider)
	{
		return $"[{Size1.ToString(format, formatProvider)}]";
	}

	public override string ToString()
	{
		return $"[{Size1}]";
	}

	public int CompareTo(object? obj)
	{
		throw new NotImplementedException();
	}

	public static T ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var size1 = double.Parse(args["s"].Value, CultureInfo.CurrentUICulture);

		return (T)Activator.CreateInstance(typeof(T), size1)!;
	}
}
