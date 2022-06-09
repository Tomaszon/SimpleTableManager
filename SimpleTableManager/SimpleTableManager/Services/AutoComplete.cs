using System;
using System.Collections.Generic;
using SimpleTableManager.Extensions;

namespace SimpleTableManager.Services;

public class AutoComplete
{
	public int AutoCompleteLength { get; private set; } = 0;

	public bool Cycling { get; set; }

	private int _index = 0;

	private List<string> _keys = new List<string>();

	public void SetKeys(IEnumerable<string> keys)
	{
		Reset();

		keys.ForEach(p => _keys.AddRange(p.Split('|')));

		_index = _keys.Count;
	}

	public string GetNextKey(string partialKey, bool backwards, out int previousAutoCompleteLength)
	{
		Cycling = true;

		StepIndex(partialKey, backwards);

		var nextKey = _keys[_index];

		previousAutoCompleteLength = AutoCompleteLength;

		AutoCompleteLength = partialKey is not null ? nextKey.Length - partialKey.Length : nextKey.Length;

		return nextKey;
	}

	private void StepIndex(string partialKey, bool backwards)
	{
		if (backwards)
		{
			_index = _index > 0 ? _index - 1 : _keys.Count - 1;
		}
		else
		{
			_index = _index < _keys.Count - 1 ? _index + 1 : 0;
		}

		if (partialKey is not null && !_keys[_index].StartsWith(partialKey))
		{
			StepIndex(partialKey, backwards);
		}
	}

	public void Reset()
	{
		if (Cycling)
		{
			AutoCompleteLength = 0;
			_index = 0;
			Cycling = false;
			_keys.Clear();
		}
	}
}
