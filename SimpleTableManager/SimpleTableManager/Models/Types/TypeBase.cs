
namespace SimpleTableManager.Models.Types;

public abstract class TypeBase<TSelf, TUnderlying>(TUnderlying value) :
	ParsableBase<TSelf>,
	IType,
	IEquatable<TSelf>
	where TSelf : TypeBase<TSelf, TUnderlying>, IParsable<TSelf>, IParsableCore<TSelf>
	where TUnderlying : IComparable
{
	public TUnderlying Value { get; init; } = value;

	public abstract string ToString(string? format, IFormatProvider? formatProvider);

	public bool Equals(TSelf? other)
	{
		if (other is null)
		{
			return false;
		}

		return Value.Equals(other.Value);
	}

	// public override bool Equals(object? obj)
	// {
	// 	if (obj is TUnderlying uv)
	// 	{
	// 		return _value.Equals(uv);
	// 	}
	// 	else if (obj is TSelf v && v is not null)
	// 	{
	// 		return _value.Equals(v._value);
	// 	}
	// 	// else if (obj is not null)
	// 	// {
	// 	// 	try
	// 	// 	{
	// 	// 		var converted = Convert.ChangeType(obj, typeof(TUnderlying));

	// 	// 		return _value.Equals(converted);
	// 	// 	}
	// 	// 	catch
	// 	// 	{
	// 	// 		return false;
	// 	// 	}
	// 	// }

	// 	return false;
	// }

	public virtual int CompareTo(object? obj)
	{
		if (obj is TUnderlying uv)
		{
			return Value.CompareTo(uv);
		}
		else if (obj is TSelf v && v is not null)
		{
			return Value.CompareTo(v.Value);
		}

		return 1;
	}

	public override string ToString()
	{
		return Value.ToString()!;
	}

	public TypeCode GetTypeCode() => TypeCode.Object;

	public object ToType(Type conversionType, IFormatProvider? provider)
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

	public virtual string ToString(IFormatProvider? provider)
	{
		return ToString()!;
	}

	public virtual bool ToBoolean(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to bool");
	}

	public virtual byte ToByte(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to byte");
	}

	public virtual char ToChar(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to char");
	}

	public virtual DateTime ToDateTime(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to DateTime");
	}

	public virtual decimal ToDecimal(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to decimal");
	}

	public virtual double ToDouble(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to double");
	}

	public virtual short ToInt16(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to short");
	}

	public virtual int ToInt32(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to int");
	}

	public virtual long ToInt64(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to long");
	}

	public virtual sbyte ToSByte(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to sbyte");
	}

	public virtual float ToSingle(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to float");
	}

	public virtual ushort ToUInt16(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to ushort");
	}

	public virtual uint ToUInt32(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to uint");
	}

	public virtual ulong ToUInt64(IFormatProvider? provider)
	{
		throw new InvalidCastException($"Can not cast {GetType().Name} to ulong");
	}

	// public static bool operator ==(TypeBase<TSelf, TUnderlying>? left, TypeBase<TSelf, TUnderlying>? right)
	// {
	// 	if (left is null && right is null)
	// 	{
	// 		return true;
	// 	}
	// 	else if (right is null || left is null)
	// 	{
	// 		return false;
	// 	}

	// 	return left._value.Equals(right._value);
	// }

	// public static bool operator !=(TypeBase<TSelf, TUnderlying>? left, TypeBase<TSelf, TUnderlying>? right)
	// {
	// 	return !(left == right);
	// }
}