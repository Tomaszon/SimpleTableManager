using System.Globalization;

namespace SimpleTableManager.Models;

[ParseFormat("size", "^(?<s>\\d+)$")]
[method: JsonConstructor]
public abstract class Shape1Sized2dBase<T>(double size1) : ParsableBase<T>, IShape2d, IShape1Sized, IParseCore<T>, IConvertible
where T : Shape1Sized2dBase<T>, IParsable<T>
{
	public double Size1 { get; set; } = size1;

	public abstract double Area { get; }

	public abstract double Perimeter { get; }

	public Shape1Sized2dBase(Shape1Sized2dBase<T> shape) : this(shape.Size1) { }

	public override string ToString()
	{
		return $"{GetType().GetFriendlyName()}({Size1})";
	}

	public static T ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var size1 = double.Parse(args["s"].Value, CultureInfo.CurrentUICulture);

		return (T)Activator.CreateInstance(typeof(T), size1)!;
	}

	public object ToType(Type conversionType, IFormatProvider? provider)
	{
		if (GetType().IsAssignableTo(conversionType))
		{
			return this;
		}
		else if (typeof(string) == conversionType)
		{
			return ToString();
		}
		else
		{
			throw new InvalidCastException($"Can not cast {GetType().Name} to {conversionType.Name}");
		}
	}

	public string ToString(IFormatProvider? provider)
	{
		return ToString();
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