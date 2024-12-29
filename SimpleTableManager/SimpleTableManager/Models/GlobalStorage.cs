namespace SimpleTableManager.Models;

/// <summary>
/// Stores items for global access
/// </summary>
public class GlobalStorage
{
	public Dictionary<string, (Type?, string)> Dictionary { get; set; } = [];

	/// <summary>
	/// Adds new item to the storage
	/// </summary>
	public void Add(string key, object? source)
	{
		var state = Shared.SerializeObject(source);

		Dictionary.Remove(key);

		Dictionary.Add(key, (source?.GetType(), state));
	}

	/// <summary>
	/// Gets a stored item
	/// </summary>
	public T? TryGet<T>(string key)
	{
		if (Dictionary.TryGetValue(key, out var state))
		{
			return (T?)Shared.DeserializeObject(state.Item1, state.Item2);
		}

		return default;
	}
}