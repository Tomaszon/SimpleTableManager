namespace SimpleTableManager.Services;

/// <summary>
/// Stores items for global access
/// </summary>
public static class GlobalContainer
{
	private static readonly Dictionary<string, string> _dictionary = new();

	/// <summary>
	/// Adds new item to the storage
	/// </summary>
	public static void Add(string key, object? source)
	{
		var state = Shared.SerializeObject(source);

		_dictionary.Remove(key);

		_dictionary.Add(key, state);
	}

	/// <summary>
	/// Gets a stored item
	/// </summary>
	public static T? TryGet<T>(string key)
	{
		if (_dictionary.TryGetValue(key, out var state))
		{
			return (T?)Shared.DeserializeObject(state);
		}

		return default;
	}
}