using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

public partial class Table
{
	[CommandReference]
	public void AddRowAt(int index)
	{
		Shared.Validate(() => index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

		Sider.Add(new IndexCell(this, index, Settings.Current.IndexCellUpArrow, Settings.Current.IndexCellDownArrow));

		for (int x = 0; x < Size.Width; x++)
		{
			var cell = new Cell(this) { ContentColor = new ConsoleColorSet(Settings.Current.DefaultContentColor) };
			//cell.PropertyChanged += CellPropertyChanged;

			Content.Insert(index * Size.Width + x, cell);
		}

		if (ViewOptions.EndPosition.Y == Size.Height - 1)
		{
			ViewOptions.IncreaseHeight();
		}

		Size.Height++;
	}

	[CommandReference]
	public void AddRowAfter(int after)
	{
		Shared.Validate(() => after >= 0 && after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

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
		Shared.Validate(() => index >= 0 && index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

		Header.Add(new IndexCell(this, index, Settings.Current.IndexCellLeftArrow, Settings.Current.IndexCellRightArrow));

		for (int y = 0; y < Size.Height; y++)
		{
			var cell = new Cell(this) { ContentColor = new ConsoleColorSet(Settings.Current.DefaultContentColor) };

			Content.Insert(Size.Width * y + y + index, cell);
			
			cell.CommandExecuted += OnCommandExecuted;
		}

		if (ViewOptions.EndPosition.X == Size.Width - 1)
		{
			ViewOptions.IncreaseWidth();
		}

		Size.Width++;
	}

	[CommandReference]
	public void AddColumnAfter(int after)
	{
		Shared.Validate(() => after >= 0 && after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

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