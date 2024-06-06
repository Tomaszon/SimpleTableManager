



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


public interface IShape2d
{
	decimal Area { get; }

	decimal Perimeter { get; }
}

[ParseFormat("size", "\\d")]
public abstract class Shape1Sized2dBase<T> : ParsableBase<T>, IShape2d where T : Shape1Sized2dBase<T>, IParsable<T>, new()
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
		return $"Size1: {Size1}";
	}

	public static T ParseWrapper(string value)
	{
		return ParseWrapper(value, args =>
		{
			var size1 = decimal.Parse(args[0].Trim());

			return new T() { Size1 = size1 };
		});
	}
}


[ParseFormat("size1,size2", "\\d,\\d"), ParseFormat("size1;size2", "\\d;\\d")]
public abstract class Shape2Sized2dBase<T> : Shape1Sized2dBase<T> where T : Shape2Sized2dBase<T>, IParsable<T>, new()
{
	public decimal Size2 { get; set; }

	public Shape2Sized2dBase() { }

	public Shape2Sized2dBase(decimal size1, decimal size2) : base(size1)
	{
		Size2 = size2;
	}

	public Shape2Sized2dBase(Shape2Sized2dBase<T> shape) : base(shape)
	{
		Size2 = shape.Size2;
	}

	public override string ToString()
	{
		return $"Size1: {Size1}, Size2: {Size2}";
	}

	public new static T ParseWrapper(string value)
	{
		return ParseWrapper(value, args =>
		{
			var size1 = decimal.Parse(args[0].Trim());
			var size2 = args.Length > 1 ? decimal.Parse(args[1].Trim()) : size1;

			return new T() { Size1 = size1, Size2 = size2 };
		});
	}
}

public class Rectangle : Shape2Sized2dBase<Rectangle>, IParsable<Rectangle>
{
	public override decimal Area => Size1 * Size2;

	public override decimal Perimeter => 2 * (Size1 + Size2);

	public Rectangle() { }

	public Rectangle(decimal size1, decimal size2) : base(size1, size2) { }

	public static Rectangle Parse(string value, IFormatProvider? provider)
	{
		return ParseWrapper(value);
	}
}

public enum Shape2dOperator
{
	Area,
	Perimeter
}
