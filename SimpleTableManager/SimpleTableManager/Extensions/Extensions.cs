namespace SimpleTableManager.Extensions;

public static class Extensions
{
	public static TOut ToType<TOut>(this IConvertible convertible)
	{
		return (TOut)convertible.ToType(typeof(TOut), null);
	}

	public static IEnumerable<T> Wrap<T>(this T value)
	{
		return [value];
	}

	public static string GetFriendlyName(this Type type)
	{
		return Shared.FRIENDLY_TYPE_NAMES.TryGetValue(type, out var value) ?
			value.ToUpperFirst() :
			type.Name.ToUpperFirst();
	}

	public static void Replace<T1, T2>(this IDictionary<T1, T2> dictionary, T1 key, T2 value)
	{
		dictionary.Remove(key);
		dictionary.Add(key, value);
	}

	public static bool NotHasFlag<T>(this T @enum, T flag)
	where T : Enum
	{
		return !@enum.HasFlag(flag);
	}
}