namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowAt([MinValue(0)] int index, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

		AddRowAtCore(index, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

		AddRowAtCore(after + 1, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowFirst([MinValue(1)] int count = 1)
	{
		AddRowAtCore(0, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowLast([MinValue(1)] int count = 1)
	{
		AddRowAtCore(Size.Height, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnAt([MinValue(0)] int index, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

		AddColumnAtCore(index, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

		AddColumnAtCore(after + 1, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnFirst([MinValue(1)] int count = 1)
	{
		AddColumnAtCore(0, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnLast([MinValue(1)] int count = 1)
	{
		AddColumnAtCore(Size.Width, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void AddRowFilter([MinValue(0)] int y, string filterExpression)
	{
		RowFilters.Replace(y, filterExpression);

		HideAndFilterCells();
	}

	[CommandFunction]
	public void AddColumnFilter([MinValue(0)] int x, string filterExpression)
	{
		ColumnFilters.Replace(x, filterExpression);

		HideAndFilterCells();
	}
}