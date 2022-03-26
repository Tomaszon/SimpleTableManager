using System;
using System.Collections.Generic;
using System.Linq;

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
	}
}
