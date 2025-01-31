using System.Globalization;

namespace SimpleTableManager.Models;

[ParseFormat("[size1*size2] (fractions)", "^\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("[size1xsize2] (fractions)", "^\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[method: JsonConstructor]
public abstract class Shape2Sized2dBase<T>(double size1, double size2) : Shape1Sized2dBase<T>(size1), IShape2Sized, IParsableCore<T>
	where T : Shape2Sized2dBase<T>, IParsable<T>
{
	public double Size2 { get; set; } = size2;

	public Shape2Sized2dBase(Shape2Sized2dBase<T> shape) :
		this(shape.Size1, shape.Size2)
	{ }

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return $"[{Size1.ToString(format, formatProvider)}x{Size2.ToString(format, formatProvider)}]";
	}

	public override string ToString()
	{
		return $"[{Size1}x{Size2}]";
	}

	public new static T ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var size1 = double.Parse(args["s"].Value, CultureInfo.CurrentUICulture);
		var size2 = args["s2"].Success ? double.Parse(args["s2"].Value, CultureInfo.CurrentUICulture) : size1;

		return (T)Activator.CreateInstance(typeof(T), size1, size2)!;
	}
}
