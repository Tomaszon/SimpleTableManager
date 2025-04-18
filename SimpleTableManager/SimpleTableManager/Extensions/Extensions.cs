﻿namespace SimpleTableManager.Extensions;

public static class Extensions
{
	public static TOut ToType<TOut>(this IConvertible convertible)
		where TOut : IConvertible
	{
		return convertible is TOut value ? value : (TOut)convertible.ToType(typeof(TOut), null);
	}

	public static IEnumerable<T> Wrap<T>(this T value)
	{
		return [value];
	}

	public static string GetFriendlyName(this Type type)
	{
		var result = Shared.FRIENDLY_TYPE_NAMES.TryGetValue(type, out var value) ? value.ToUpperFirst() : type.Name.ToUpperFirst();

		return result.IndexOf('`') is var index && index != -1 ? result[..index] : result;
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

	public static Exception GetInnermostException(this Exception ex)
	{
		return ex.InnerException is null ? ex : ex.InnerException.GetInnermostException();
	}

	public static ReadOnlySpan<char> ToReadOnlySpan(this char c, int count)
	{
		return new ReadOnlySpan<char>([.. Enumerable.Repeat(c, count)]);
	}
}