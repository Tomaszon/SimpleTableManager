namespace SimpleTableManager.Services;

public class CommandHistory
{
	private int _index = 0;

	private List<string> _history = new List<string>();

	public bool TryGetPreviousHistoryItem([NotNullWhen(true)] out string command)
	{
		if (_index > 0)
		{
			_index--;
			command = _history[_index];

			return true;
		}

		command = null;

		return false;
	}

	public bool TryGetNextHistoryItem(out string command)
	{
		if (_index < _history.Count)
		{
			_index++;
			command = _index < _history.Count ? _history[_index] : null;

			return true;
		}

		command = null;

		return false;
	}

	public void Add(string command)
	{
		_history.Add(command);

		if (_history.Count > Settings.Current.CommandHistorySize)
		{
			_history.RemoveAt(0);
		}

		ResetCycle();
	}

	public void ResetCycle()
	{
		_index = _history.Count;
	}
}
