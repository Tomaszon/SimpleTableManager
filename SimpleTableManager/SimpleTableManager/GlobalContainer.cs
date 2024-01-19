namespace SimpleTableManager.Services;

public static class GlobalContainer
{
	private static readonly Dictionary<string, string> _dictionary = new();

	public static void Add(string key, object? source)
	{
		var state = Shared.SerializeObject(source);

		_dictionary.Remove(key);

		_dictionary.Add(key, state);
	}

	public static T? TryGet<T>(string key)
	where T: class
	{
		if (_dictionary.TryGetValue(key, out var state))
		{
			return (T?)Shared.DeserializeObject(state);
		}

		return null;
	}
}