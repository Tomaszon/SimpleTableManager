using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandReference]
	public void AddRowAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

		Sider.Add(new IndexCell(this, IndexCellType.Sider, index, Settings.Current.IndexCellUpArrow, Settings.Current.IndexCellDownArrow));

		for (int x = 0; x < Size.Width; x++)
		{
			AddNewContentCellAt(index * Size.Width + x);
		}

		Size.Height++;

		if (ViewOptions.EndPosition.Y == Size.Height - 2)
		{
			ViewOptions.IncreaseHeight();
		}
	}

	[CommandReference]
	public void AddRowAfter(int after)
	{
		ThrowIfNot(after >= 0 && after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

		AddRowAt(after + 1);
	}

	[CommandReference]
	public void AddRowFirst()
	{
		AddRowAt(0);
	}

	[CommandReference]
	public void AddRowLast()
	{
		AddRowAt(Size.Height);
	}

	[CommandReference]
	public void AddColumnAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

		Header.Add(new IndexCell(this, IndexCellType.Header, index, Settings.Current.IndexCellLeftArrow, Settings.Current.IndexCellRightArrow));

		for (int y = 0; y < Size.Height; y++)
		{
			AddNewContentCellAt(Size.Width * y + y + index);
		}

		Size.Width++;

		if (ViewOptions.EndPosition.X == Size.Width - 2)
		{
			ViewOptions.IncreaseWidth();
		}
	}

	[CommandReference]
	public void AddColumnAfter(int after)
	{
		ThrowIfNot(after >= 0 && after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

		AddColumnAt(after + 1);
	}

	[CommandReference]
	public void AddColumnFirst()
	{
		AddColumnAt(0);
	}

	[CommandReference]
	public void AddColumnLast()
	{
		AddColumnAt(Size.Width);
	}
}