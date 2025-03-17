namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void SetName([MinLength(1)] string name)
	{
		Name = name;
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void SetSize(Size size)
	{
		ThrowIfNot(size.Width > 0 && size.Height > 0, "Can not decrease table size under 1 column or 1 row!");

		var widthDifference = size.Width - Size.Width;
		var heightDifference = size.Height - Size.Height;

		if (widthDifference < 0)
		{
			RemoveColumnAtCore(Size.Width + widthDifference, -widthDifference);
		}
		else if (widthDifference > 0)
		{
			AddColumnAtCore(Size.Width, widthDifference);
		}

		if (heightDifference < 0)
		{
			RemoveRowAtCore(Size.Height + heightDifference, -heightDifference);
		}
		else if (heightDifference > 0)
		{
			AddRowAtCore(Size.Height, heightDifference);
		}

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void SetColumnWidth([MinValue(0)] int index, [MinValue(0)] int width)
	{
		ColumnAt(index).ForEach(c => c.GivenSize = new(width, c.GivenSize.Height));
	}

	[CommandFunction]
	public void SetRowHeight([MinValue(0)] int index, [MinValue(0)] int height)
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
	public void SetViewOptionsColumns([MinValue(0)] int x1, [MinValue(0)] int x2)
	{
		SetViewOptions(new Position(x1, ViewOptions.StartPosition.Y), new Position(x2, ViewOptions.EndPosition.Y));

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void SetViewOptionsRows([MinValue(0)] int y1, [MinValue(0)] int y2)
	{
		SetViewOptions(new Position(ViewOptions.StartPosition.X, y1), new Position(ViewOptions.EndPosition.X, y2));

		ViewOptions.InvokeViewChangedEvent();
	}
}