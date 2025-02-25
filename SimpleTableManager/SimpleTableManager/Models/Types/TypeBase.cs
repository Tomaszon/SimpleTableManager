namespace SimpleTableManager.Models.Types;

public abstract class TypeBase<T, TUnderlying>(TUnderlying value) :
	ParsableBase<T>,
	IType
	where T : TypeBase<T, TUnderlying>, IParsable<T>, IParsableCore<T>
	where TUnderlying : IComparable
{
	protected readonly TUnderlying _value = value;

	public abstract string ToString(string? format, IFormatProvider? formatProvider);

	public virtual DateTime ToDateTime(IFormatProvider? provider)
	{
		return ((IType)this).ToDateTime(provider);
	}

	public virtual int ToInt32(IFormatProvider? provider)
	{
		return ((IType)this).ToInt32(provider);
	}

	public virtual long ToInt64(IFormatProvider? provider)
	{
		return ((IType)this).ToInt64(provider);
	}

	public virtual bool ToBoolean(IFormatProvider? provider)
	{
		return ((IType)this).ToBoolean(provider);
	}

	public virtual double ToDouble(IFormatProvider? provider)
	{
		return ((IType)this).ToDouble(provider);
	}

	public override bool Equals(object? obj)
	{
		if (obj is TUnderlying uv)
		{
			return _value.Equals(uv);
		}
		else if (obj is T v && v is not null)
		{
			return _value.Equals(v._value);
		}

		return false;
	}

	[ExcludeFromCodeCoverage]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return _value.ToString()!;
	}

	public virtual int CompareTo(object? obj)
	{
		if (obj is TUnderlying uv)
		{
			return _value.CompareTo(uv);
		}
		else if (obj is T v && v is not null)
		{
			return _value.CompareTo(v._value);
		}

		return 1;
	}
}