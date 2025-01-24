namespace SimpleTableManager.Models;

public abstract class ConvertibleBase<T> : ParsableBase<T>, IConvertible
	where T : class, IParsable<T>, IParseCore<T>
{
	public object ToType(Type conversionType, IFormatProvider? provider)
	{
		if (GetType().IsAssignableTo(conversionType))
		{
			return this;
		}
		else if (typeof(string) == conversionType)
		{
			return ToString()!;
		}
		else
		{
			throw new InvalidCastException($"Can not cast {GetType().Name} to {conversionType.Name}");
		}
	}

	public string ToString(IFormatProvider? provider)
	{
		return ToString()!;
	}

	public TypeCode GetTypeCode()
	{
		return TypeCode.Object;
	}

	public bool ToBoolean(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to bool");
	}

	public byte ToByte(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to byte");
	}

	public char ToChar(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to char");
	}

	public DateTime ToDateTime(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to DateTime");
	}

	public decimal ToDecimal(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to decimal");
	}

	public double ToDouble(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to double");
	}

	public short ToInt16(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to short");
	}

	public int ToInt32(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to int");
	}

	public long ToInt64(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to long");
	}

	public sbyte ToSByte(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to sbyte");
	}

	public float ToSingle(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to float");
	}

	public ushort ToUInt16(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to ushort");
	}

	public uint ToUInt32(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to uint");
	}

	public ulong ToUInt64(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to ulong");
	}
}