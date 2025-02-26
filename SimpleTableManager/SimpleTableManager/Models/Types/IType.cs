namespace SimpleTableManager.Models.Types;

public interface IType :
	IConvertible,
	IFormattable,
	IComparable
{
	public static IntegerType From(long value)
	{
		return new IntegerType(value);
	}

	TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

	object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
	{
		if (GetType().IsAssignableTo(conversionType))
		{
			return this;
		}

		return conversionType switch
		{
			Type t when t == typeof(bool) => ToBoolean(provider),
			Type t when t == typeof(long) => ToInt64(provider),
			Type t when t == typeof(double) => ToDouble(provider),
			Type t when t == typeof(char) => ToChar(provider),
			Type t when t == typeof(string) => ToString(provider),
			Type t when t == typeof(DateTime) => ToDateTime(provider),

			_ => throw new InvalidCastException($"Can not cast {GetType().Name} to {conversionType.Name}")
		};
	}

	string IConvertible.ToString(IFormatProvider? provider)
	{
		return ToString()!;
	}

	bool IConvertible.ToBoolean(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to bool");
	}

	byte IConvertible.ToByte(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to byte");
	}

	char IConvertible.ToChar(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to char");
	}

	DateTime IConvertible.ToDateTime(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to DateTime");
	}

	decimal IConvertible.ToDecimal(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to decimal");
	}

	double IConvertible.ToDouble(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to double");
	}

	short IConvertible.ToInt16(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to short");
	}

	int IConvertible.ToInt32(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to int");
	}

	long IConvertible.ToInt64(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to long");
	}

	sbyte IConvertible.ToSByte(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to sbyte");
	}

	float IConvertible.ToSingle(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to float");
	}

	ushort IConvertible.ToUInt16(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to ushort");
	}

	uint IConvertible.ToUInt32(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to uint");
	}

	ulong IConvertible.ToUInt64(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to ulong");
	}
}