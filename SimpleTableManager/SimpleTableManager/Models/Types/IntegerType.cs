namespace SimpleTableManager.Models.Types;

[ParseFormat("+-value", "^-?\\d+$")]
public class IntegerType(long value) :
	TypeBase<IntegerType, long>(value),
	INumericType<IntegerType, long>,
	IParsable<IntegerType>,
	IParsableCore<IntegerType>
{
	// public static IntegerType MaxValue => throw new NotImplementedException();

	// public static IntegerType MinValue => throw new NotImplementedException();

	// public static IntegerType AdditiveIdentity => 0;

	// public static IntegerType MultiplicativeIdentity => 1;


	public static IntegerType Abs(IntegerType value)
	{
		return new(long.Abs(value._value));
	}

	public static IntegerType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var value = args.Values.Single(g => g.Success && g.Name != "0").Value;

		return long.Parse(value);
	}

	public static implicit operator long(IntegerType value)
	{
		return value._value;
	}

	public static implicit operator IntegerType(long value)
	{
		return new(value);
	}

	public static IntegerType operator +(IntegerType left, IntegerType right)
	{
		return new(left._value + right._value);
	}

	public static IntegerType operator -(IntegerType left, IntegerType right)
	{
		return new(left._value - right._value);
	}

	public static IntegerType operator *(IntegerType left, IntegerType right)
	{
		return new(left._value * right._value);
	}

	public static IntegerType operator /(IntegerType left, IntegerType right)
	{
		return new(left._value / right._value);
	}

	public static IntegerType operator -(IntegerType value)
	{
		return new(-value._value);
	}

	public override bool ToBoolean(IFormatProvider? provider)
	{
		return this != 0;
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return this;
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