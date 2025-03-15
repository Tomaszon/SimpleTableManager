namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveRowAt([MinValue(0)] int index)
	{
		ThrowIfNot(index <= Size.Height - 1, $"Index is not in the needed range: [0, {Size.Height - 1}]");
		ThrowIfNot(Size.Height > 1, "Can not decrease table height under 1 row!");

		var selectedCells = Content.Where(c => c.Selection.IsPrimarySelected).ToList();

		DeselectCells(selectedCells);

		Content.RemoveRange(index * Size.Width, Size.Width);
		Sider.RemoveAt(index);

		Size.Height--;

		if (ViewOptions.EndPosition.Y == Size.Height)
		{
			ViewOptions.DecreaseHeight();
		}
		else
		{
			ViewOptions.InvokeViewChangedEvent();
		}

		RemoveDeadCellReferences();

		SelectCells(selectedCells);
	}


	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveRowAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

		Shared.IndexArray(count).ForEach(i => RemoveRowAt(after + 1));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveFirstRow([MinValue(1)] int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => RemoveRowAt(0));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveLastRow([MinValue(1)] int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => RemoveRowAt(Size.Height - 1));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveColumnAt([MinValue(0)] int index)
	{
		ThrowIfNot(index <= Size.Width - 1, $"Index is not in the needed range: [0, {Size.Width - 1}]");
		ThrowIfNot(Size.Width > 1, "Can not decrease table width under 1 column!");

		var selectedCells = Content.Where(c => c.Selection.IsPrimarySelected).ToList();

		DeselectCells(selectedCells);

		for (int y = 0; y < Size.Height; y++)
		{
			Content.RemoveAt(Size.Width * y - y + index);
		}
		Header.RemoveAt(index);

		Size.Width--;

		if (ViewOptions.EndPosition.X == Size.Width)
		{
			ViewOptions.DecreaseWidth();
		}
		else
		{
			ViewOptions.InvokeViewChangedEvent();
		}

		RemoveDeadCellReferences();

		SelectCells(selectedCells);
	}

	private void RemoveDeadCellReferences()
	{
		Content.Where(c =>
			c.ReferencedObject is not null &&
			!Content.Contains((Cell)c.ReferencedObject)).ForEach(c => c.ResetReferenceCell());
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveColumnAfter([MinValue(0)] int after, [MinValue(1)] int count = 1)
	{
		ThrowIfNot(after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

		Shared.IndexArray(count).ForEach(i => RemoveColumnAt(after + 1));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveFirstColumn([MinValue(1)] int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => RemoveColumnAt(0));
	}

	[CommandFunction(ClearsCache = true, Clears = GlobalStorageKey.CellContent)]
	public void RemoveLastColumn([MinValue(1)] int count = 1)
	{
		Shared.IndexArray(count).ForEach(i => RemoveColumnAt(Size.Width - 1));
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