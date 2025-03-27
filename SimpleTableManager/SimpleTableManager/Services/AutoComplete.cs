namespace SimpleTableManager.Services;

public class AutoComplete
{
	private int _autoCompleteLength = 0;

	public bool Cycling { get; set; }

	private int _index = 0;

	private readonly List<string> _keys = [];

	public void SetKeys(IEnumerable<string>? keys)
	{
		Reset();

		if (keys is not null)
		{
			keys.ForEach(p => _keys.Add(p));

			_index = _keys.Count;
		}
	}

	public string GetNextKey(string? partialKey, bool backwards, out int previousAutoCompleteLength, out int matchingKeyCount)
	{
		Cycling = true;

		StepIndex(partialKey, backwards);

		var nextKey = _keys[_index];

		previousAutoCompleteLength = _autoCompleteLength;

		_autoCompleteLength = nextKey.Length - (partialKey?.Length ?? 0);

		matchingKeyCount = _keys.Count(k => k.StartsWith(partialKey ?? "", StringComparison.OrdinalIgnoreCase));

		return nextKey;
	}

	private void StepIndex(string? partialKey, bool backwards)
	{
		_index = backwards ? _index > 0 ? _index - 1 : _keys.Count - 1 : _index < _keys.Count - 1 ? _index + 1 : 0;

		if (partialKey is not null && !_keys[_index].StartsWith(partialKey, StringComparison.OrdinalIgnoreCase))
		{
			StepIndex(partialKey, backwards);
		}
	}

	public string TrimValueForCycling(string value)
	{
		return value[..^_autoCompleteLength];
	}

	public void Reset()
	{
		if (Cycling)
		{
			_autoCompleteLength = 0;
			_index = 0;
			Cycling = false;
			_keys.Clear();
		}
	}
}
