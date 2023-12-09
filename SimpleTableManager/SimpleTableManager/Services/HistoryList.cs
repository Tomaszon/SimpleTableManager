namespace SimpleTableManager.Services;

public class HistoryList
{
	private int _index = 0;

	private readonly List<string> _history = new();

	private readonly uint _length = 1;

	private readonly int _indexResetOffset;

	private readonly bool _cleanAfterInsert;

	public HistoryList(uint length, bool cleanAfterInsert = false, int indexResetOffset = 0)
	{
		_length = length;
		_indexResetOffset = indexResetOffset;
		_cleanAfterInsert = cleanAfterInsert;
	}

	public bool TryGetPreviousHistoryItem([NotNullWhen(true)] out string? element)
	{
		if (_index > 0)
		{
			_index--;
			element = _history[_index];

			return true;
		}

		element = null;

		return false;
	}

	public bool TryGetNextHistoryItem([NotNullWhen(true)] out string? element)
	{
		if (_index < _history.Count - 1)
		{
			_index++;
			element = _history[_index];

			return true;
		}

		element = null;

		return false;
	}

	public void Add(string element)
	{
		var currentIndex = _index;

		if(_cleanAfterInsert)
		{
			ResetCycle();

			while(_history.Count - 1 > currentIndex)
			{
				_history.RemoveAt(_history.Count - 1);
			}
		}

		_history.Add(element);

		if (_history.Count > _length)
		{
			_history.RemoveAt(0);
		}

		ResetCycle();
	}

	public void ResetCycle()
	{
		_index = _history.Count + _indexResetOffset;
	}
}