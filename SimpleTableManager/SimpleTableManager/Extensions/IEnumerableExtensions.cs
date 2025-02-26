namespace SimpleTableManager.Extensions;

using System.Collections;

public static class IEnumerableExtensions
{
	public static IEnumerable<T2> CastTo<T2>(this IEnumerable collection)
	{
		foreach (var item in collection)
		{
			yield return (T2)item;
		}
	}

	public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
	{
		foreach (var item in collection)
		{
			action(item);
		}
	}

	public static IEnumerable<T> TakeAround<T>(this IEnumerable<T> collection, int take, int takeLast)
	{
		int count = collection.Count();

		if (take <= 0 && takeLast <= 0)
		{
			yield break;
		}

		for (int i = 0; i < count; i++)
		{
			if (take == int.MaxValue && takeLast == int.MaxValue || take < int.MaxValue && i < take || takeLast < int.MaxValue && i >= count - takeLast)
			{
				yield return collection.ElementAt(i);
			}
		}
	}
}