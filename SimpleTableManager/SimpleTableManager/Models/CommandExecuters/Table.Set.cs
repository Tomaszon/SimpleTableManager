namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void SetName(string name)
	{
		Name = name;
	}

	[CommandFunction(GlobalCacheClearNeeded = true)]
	public void SetSize(Size size)
	{
		ThrowIfNot(size.Width > 0 && size.Height > 0, "Can not decrease table size under 1 column or 1 row!");

		while (Size.Width != size.Width)
		{
			if (Size.Width > size.Width)
			{
				RemoveLastColumn();
			}
			else
			{
				AddColumnLast();
			}
		}
		while (Size.Height != size.Height)
		{
			if (Size.Height > size.Height)
			{
				RemoveLastRow();
			}
			else
			{
				AddRowLast();
			}
		}
	}

	[CommandFunction]
	public void SetColumnWidth(int index, int width)
	{
		ColumnAt(index).ForEach(c => c.GivenSize = new(width, c.GivenSize.Height));
	}

	[CommandFunction]
	public void SetRowHeight(int index, int height)
	{
		RowAt(index).ForEach(c => c.GivenSize = new(c.GivenSize.Width, height));
	}

	[CommandFunction]
	public void SetViewOptions(Position from, Position to)
	{
		ThrowIfNot(from.X >= 0 && from.X <= to.X, $"Index x1 is not in the needed range: [0, {to.X}]");
		ThrowIfNot(to.X < Size.Width, $"Index x2 is not in the needed range: [{from.X}, {Size.Width - 1}]");

		ThrowIfNot(from.Y >= 0 && from.Y <= to.Y, $"Index y1 is not in the needed range: [0, {to.Y}]");
		ThrowIfNot(to.Y < Size.Height, $"Index y2 is not in the needed range: [{from.Y}, {Size.Height - 1}]");

		ViewOptions.StartPosition = from;
		ViewOptions.EndPosition = to;

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void SetViewOptionsColumns(int x1, int x2)
	{
		SetViewOptions(new Position(x1, ViewOptions.StartPosition.Y), new Position(x2, ViewOptions.EndPosition.Y));

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void SetViewOptionsRows(int y1, int y2)
	{
		SetViewOptions(new Position(ViewOptions.StartPosition.X, y1), new Position(ViewOptions.EndPosition.X, y2));

		ViewOptions.InvokeViewChangedEvent();
	}
}