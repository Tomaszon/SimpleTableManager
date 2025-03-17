namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveRowAt([MinValue(0)] int index, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(index <= Size.Height - 1, $"Index is not in the needed range: [0, {Size.Height - 1}]");
		ThrowIfNot(Size.Height > count, "Can not decrease table height under 1 row!");
		ThrowIfNot(Size.Height > index + count, $"No {count} columns at and after the given index");

		RemoveRowAtCore(index, count);

		ViewOptions.InvokeViewChangedEvent();
	}


	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveRowAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");
		ThrowIfNot(Size.Height > count, "Can not decrease table height under 1 row!");
		ThrowIfNot(Size.Height > after + count + 1, $"No {count} columns after the given index");

		RemoveRowAtCore(after + 1, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveFirstRow([MinValue(1)] int count = 1)
	{
		ThrowIfNot(Size.Height > count, "Can not decrease table height under 1 row!");

		RemoveRowAtCore(0, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveLastRow([MinValue(1)] int count = 1)
	{
		ThrowIfNot(Size.Height > count, "Can not decrease table height under 1 row!");

		RemoveRowAtCore(Size.Height - count, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveColumnAt([MinValue(0)] int index, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(index <= Size.Width - 1, $"Index is not in the needed range: [0, {Size.Width - 1}]");
		ThrowIfNot(Size.Width > count, "Can not decrease table width under 1 column!");
		ThrowIfNot(Size.Width > index + count, $"No {count} rows at and after the given index");

		RemoveColumnAtCore(index, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveColumnAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");
		ThrowIfNot(Size.Width > count, "Can not decrease table width under 1 column!");
		ThrowIfNot(Size.Width > after + count + 1, $"No {count} rows after the given index");

		RemoveColumnAtCore(after + 1, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveFirstColumn([MinValue(1)] int count = 1)
	{
		ThrowIfNot(Size.Width > count, "Can not decrease table width under 1 column!");

		RemoveColumnAtCore(0, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveLastColumn([MinValue(1)] int count = 1)
	{
		ThrowIfNot(Size.Width > count, "Can not decrease table width under 1 column!");

		RemoveColumnAtCore(Size.Width - count, count);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void RemoveRowFilter([MinValue(0)] int y)
	{
		RowFilters.Remove(y);

		ApplyFilters();
	}

	[CommandFunction]
	public void RemoveColumnFilter([MinValue(0)] int x)
	{
		ColumnFilters.Remove(x);

		ApplyFilters();
	}

	[CommandFunction]
	public void RemoveRowFilters()
	{
		RowFilters.Clear();

		ApplyFilters();
	}

	[CommandFunction]
	public void RemoveColumnFilters()
	{
		ColumnFilters.Clear();

		ApplyFilters();
	}

	[CommandFunction]
	public void RemoveFilters()
	{
		ColumnFilters.Clear();
		RowFilters.Clear();

		ApplyFilters();
	}
}