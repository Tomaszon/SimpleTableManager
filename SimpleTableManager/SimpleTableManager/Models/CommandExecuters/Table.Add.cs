using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void AddRowAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

		Sider.Insert(index, new IndexCell(this, IndexCellType.Sider, index, Settings.Current.IndexCellUpArrow, Settings.Current.IndexCellDownArrow));

		Shared.IndexArray(Header.Count).ForEach(i => Header[i].Index = i);

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

	[CommandFunction]
	public void AddRowAfter(int after, int count = 1)
	{
		ThrowIfNot(after >= 0 && after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

		Shared.IndexArray(count).ForEach(i => AddRowAt(after + 1));
	}

	[CommandFunction]
	public void AddRowFirst(int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddRowAt(0));
	}

	[CommandFunction]
	public void AddRowLast(int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddRowAt(Size.Height));
	}

	[CommandFunction]
	public void AddColumnAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

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

	[CommandFunction]
	public void AddColumnAfter(int after, int count = 1)
	{
		ThrowIfNot(after >= 0 && after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

		Shared.IndexArray(count).ForEach(i => AddColumnAt(after + 1));
	}

	[CommandFunction]
	public void AddColumnFirst(int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddColumnAt(0));
	}

	[CommandFunction]
	public void AddColumnLast(int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => AddColumnAt(Size.Width));
	}

	[CommandFunction]
	public void AddRowFilter(int y, string filterExpression)
	{
		RowFilters.Replace(y, filterExpression);

		ApplyFilters();
	}

	[CommandFunction]
	public void AddColumnFilter(int x, string filterExpression)
	{
		ColumnFilters.Replace(x, filterExpression);

		ApplyFilters();
	}
}