namespace SimpleTableManager.Models;

public interface IParseCore<T>
{
	static abstract T ParseCore(GroupCollection groupCollection, IFormatProvider? formatProvider = null);
}