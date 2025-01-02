namespace SimpleTableManager.Models;

/// <summary>
/// Stores items for global access
/// </summary>
public class GlobalStorage
{
	public Dictionary<GlobalStorageKey, (Type? type, string? value)> Dictionary { get; set; } = [];

	/// <summary>
	/// Adds new item to the storage
	/// </summary>
	public void Add(GlobalStorageKey key, object? source)
	{
		Dictionary.Remove(key);

		if (source is not null)
		{
			var state = Shared.SerializeObject(source);

			Dictionary.Add(key, (source?.GetType(), state));
		}
	}

	/// <summary>
	/// Gets a stored item
	/// </summary>
	public T? TryGet<T>(GlobalStorageKey key)
	{
		if (Dictionary.TryGetValue(key, out var state))
		{
			return (T?)Shared.DeserializeObject(state.type, state.value);
		}

		return default;
	}

	/// <summary>
	/// Removes a stored item
	/// </summary>
	public void Remove(GlobalStorageKey key)
	{
		Dictionary.Remove(key);
	}
}