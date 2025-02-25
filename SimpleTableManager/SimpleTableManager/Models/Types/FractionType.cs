namespace SimpleTableManager.Models.Types;

[ParseFormat("+-value", "^-?\\d+$")]
[ParseFormat("+-value.decimals", "^-?\\d*.\\d+$")]
public class FractionType(double value) :
	TypeBase<FractionType, double>(value),
	INumericType<FractionType, double>,
	IParsable<FractionType>,
	IParsableCore<FractionType>
{
	public static FractionType Abs(FractionType value)
	{
		return new(double.Abs(value._value));
	}

	public static FractionType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var value = args.Values.Single(g => g.Success && g.Name != "0").Value;

		return double.Parse(value);
	}

	public static implicit operator double(FractionType value)
	{
		return value._value;
	}

	public static implicit operator FractionType(double value)
	{
		return new(value);
	}

	public static FractionType operator +(FractionType left, FractionType right)
	{
		return new(left._value + right._value);
	}

	public static FractionType operator -(FractionType left, FractionType right)
	{
		return new(left._value - right._value);
	}

	public static FractionType operator *(FractionType left, FractionType right)
	{
		return new(left._value * right._value);
	}

	public static FractionType operator /(FractionType left, FractionType right)
	{
		return new(left._value / right._value);
	}

	public static FractionType operator -(FractionType value)
	{
		return new(-value._value);
	}

	public override bool ToBoolean(IFormatProvider? provider)
	{
		return this != 0;
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return (long)this;
	}

	public override double ToDouble(IFormatProvider? provider)
	{
		return this;
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(format, formatProvider);
	}
}