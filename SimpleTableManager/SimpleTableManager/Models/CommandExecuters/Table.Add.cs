namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

		Sider.Insert(index, new IndexCell(this, IndexCellType.Sider, index, Settings.Current.IndexCellUpArrow, Settings.Current.IndexCellDownArrow));

		Shared.IndexArray(Sider.Count).ForEach(i => Sider[i].Index = i);

		for (int x = 0; x < Size.Width; x++)
		{
			AddNewContentCell(index * Size.Width + x);
		}

		Size.Height++;

		if (ViewOptions.EndPosition.Y == Size.Height - 2)
		{
			ViewOptions.IncreaseHeight();
		}
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

		Shared.IndexArray(count).ForEach(i => AddRowAt(after + 1));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowFirst(int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddRowAt(0));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddRowLast(int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddRowAt(Size.Height));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnAt([MinValue(0)] int index)
	{
		ThrowIfNot(index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

		Header.Insert(index, new IndexCell(this, IndexCellType.Header, index, Settings.Current.IndexCellLeftArrow, Settings.Current.IndexCellRightArrow));

		Shared.IndexArray(Header.Count).ForEach(i => Header[i].Index = i);

		for (int y = 0; y < Size.Height; y++)
		{
			AddNewContentCell(Size.Width * y + y + index);
		}

		Size.Width++;

		if (ViewOptions.EndPosition.X == Size.Width - 2)
		{
			ViewOptions.IncreaseWidth();
		}
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

		Shared.IndexArray(count).ForEach(i => AddColumnAt(after + 1));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnFirst([MinValue(1)] int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddColumnAt(0));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void AddColumnLast([MinValue(1)] int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddColumnAt(Size.Width));
	}

	[CommandFunction]
	public void AddRowFilter([MinValue(0)] int y, string filterExpression)
	{
		RowFilters.Replace(y, filterExpression);

		ApplyFilters();
	}

	[CommandFunction]
	public void AddColumnFilter([MinValue(0)] int x, string filterExpression)
	{
		ColumnFilters.Replace(x, filterExpression);

		ApplyFilters();
	}
}