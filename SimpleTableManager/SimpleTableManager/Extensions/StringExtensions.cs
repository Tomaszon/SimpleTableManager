namespace SimpleTableManager.Extensions;

public static class StringExtensions
{
	public static string PadLeftRight(this string value, int totalWidth)
	{
		int leftPadding = (totalWidth - value.Length) / 2;

		return value.PadLeft(value.Length + leftPadding).PadRight(totalWidth);
	}

	public static string AppendLeft(this string value, char c, int count)
	{
		return $"{c.ToReadOnlySpan(count)}{value}";
	}

	public static string AppendRight(this string value, char c, int count)
	{
		return $"{value}{c.ToReadOnlySpan(count)}";
	}

	public static string AppendLeftRight(this string value, char c, int countToLeft, int countToRight)
	{
		return $"{c.ToReadOnlySpan(countToLeft)}{value}{c.ToReadOnlySpan(countToRight)}";
	}

	public static string ToUpperFirst(this string value)
	{
		return $"{char.ToUpper(value.First())}{string.Concat(value.ToArray()[1..])}";
	}
}