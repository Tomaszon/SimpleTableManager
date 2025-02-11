namespace SimpleTableManager.Models;

public abstract class ConvertibleBase<T> :
	ParsableBase<T>,
	IConvertibleBase
	where T : class, IParsable<T>, IParsableCore<T>
{
	public virtual DateTime ToDateTime(IFormatProvider? provider)
	{
		return ((IConvertibleBase)this).ToDateTime(provider);
	}

	public virtual int ToInt32(IFormatProvider? provider)
	{
		return ((IConvertibleBase)this).ToInt32(provider);
	}

	public virtual long ToInt64(IFormatProvider? provider)
	{
		return ((IConvertibleBase)this).ToInt64(provider);
	}

	public virtual bool ToBoolean(IFormatProvider? provider)
	{
		return ((IConvertibleBase)this).ToBoolean(provider);
	}

	public virtual double ToDouble(IFormatProvider? provider)
	{
		return ((IConvertibleBase)this).ToDouble(provider);
	}
}