namespace SimpleTableManager.Models;

public interface IParsableCore<T>
{
	static abstract T ParseCore(GroupCollection args, IFormatProvider? formatProvider = null);
}