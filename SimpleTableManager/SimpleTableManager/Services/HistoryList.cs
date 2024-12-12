using System.ComponentModel;

namespace SimpleTableManager.Services;

/// <summary>
/// Stores and iterates on stored items
/// </summary>
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

	public void Init(string element)
	{
		Clear();

		Add(element);
	}

	/// <summary>
	/// Gets prevoius stored item
	/// </summary>
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

	/// <summary>
	/// Gets next stored item
	/// </summary>
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

	/// <summary>
	/// Stores items to iterate through
	/// </summary>
	public void Add(string element)
	{
		var currentIndex = _index;

		if (_cleanAfterInsert)
		{
			ResetCycle();

			while (_history.Count - 1 > currentIndex)
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

	/// <summary>
	/// Resets item iteration
	/// </summary>
	public void ResetCycle()
	{
		_index = _history.Count + _indexResetOffset;
	}

	/// <summary>
	/// Clears all stored items
	/// </summary>
	public void Clear()
	{
		_history.Clear();
		_index = 0;
	}
}