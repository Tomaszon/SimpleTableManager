namespace SimpleTableManager.Extensions;

using System.Collections;

public static class IEnumerableExtensions
{
	public static List<T> ConvertAll<TFrom, T>(this TFrom[] collection, Converter<TFrom, T> converter)
	{
		var list = new List<TFrom>(collection);
		
		return list.ConvertAll(converter);
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