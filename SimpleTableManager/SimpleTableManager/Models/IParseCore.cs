using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

public interface IParseCore<T>
{
	static abstract T ParseCore(GroupCollection groupCollection);
}