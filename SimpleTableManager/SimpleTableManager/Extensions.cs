using System;
using System.Collections.Generic;

namespace SimpleTableManager.Extensions
{
	public static class Extensions
	{
		public static string PadLeftRight(this string value, int totalWidth)
		{
			int leftPadding = (totalWidth - value.Length) / 2;

			value = value.PadLeft(value.Length + leftPadding);
			value = value.PadRight(totalWidth);

			return value;
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
			{
				action(item);
			}
		}

		public static string AppendLeft(this string value, char c, int count)
		{
			return new string(c, count) + value;
		}
		public static string AppendRight(this string value, char c, int count)
		{
			return value + new string(c, count);
		}

		public static string AppendLeftRight(this string value, char c, int countToLeft, int countToRight)
		{
			return value.AppendLeft(c, countToLeft).AppendRight(c, countToRight);
		}
	}
}
