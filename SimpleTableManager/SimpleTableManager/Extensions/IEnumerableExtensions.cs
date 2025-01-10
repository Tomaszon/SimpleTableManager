namespace SimpleTableManager.Extensions;

public static class IEnumerableExtensions
{
	public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
	{
		foreach (var item in collection)
		{
			action(item);
		}
	}

	public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action, int delay)
	{
		foreach (var item in collection)
		{
			action(item);

			Task.Delay(delay).Wait();
		}
	}
}