namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void SelectCell(Position position, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		this[position].IsSelected = true;
	}

	[CommandFunction]
	public void SelectCells(params Position[] positions)
	{
		ThrowIfNot<TargetParameterCountException>(positions.Length > 0, "One or more positions needed!");

		positions.ForEach(p => this[p].IsSelected = true);
	}

	[CommandFunction]
	public void SelectCellRange(Position positionFrom, Position positionTo, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		this[positionFrom, positionTo].ForEach(c => c.IsSelected = true);
	}

	[CommandFunction]
	public void SelectColumn(int x, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		Columns[x].ForEach(c => c.IsSelected = true);
	}

	[CommandFunction]
	public void SelectRow(int y, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		Rows[y].ForEach(c => c.IsSelected = true);
	}

	[CommandFunction, CommandShortcut("selectAllCells")]
	public void SelectAll()
	{
		Content.ForEach(c => c.IsSelected = true);
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionRight()
	{
		var selectedCells = GetSelectedCells();

		ThrowIf(selectedCells.Count() != 1, "Selection movement available with one selected cell!");

		var cell = selectedCells.Single();

		var position = this[cell];

		if (position.X < Size.Width - 1)
		{
			SelectCell(new Position(position.X + 1, position.Y), true);
		}
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionLeft()
	{
		var selectedCells = GetSelectedCells();

		ThrowIf(selectedCells.Count() != 1, "Selection movement available with one selected cell!");

		var cell = selectedCells.Single();

		var position = this[cell];

		if (position.X > 0)
		{
			SelectCell(new Position(position.X - 1, position.Y), true);
		}
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionDown()
	{
		var selectedCells = GetSelectedCells();

		ThrowIf(selectedCells.Count() != 1, "Selection movement available with one selected cell!");

		var cell = selectedCells.Single();

		var position = this[cell];

		if (position.Y < Size.Height - 1)
		{
			SelectCell(new Position(position.X, position.Y + 1), true);
		}
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionUp()
	{
		var selectedCells = GetSelectedCells();

		ThrowIf(selectedCells.Count() != 1, "Selection movement available with one selected cell!");

		var cell = selectedCells.Single();

		var position = this[cell];

		if (position.Y > 0)
		{
			SelectCell(new Position(position.X, position.Y - 1), true);
		}
	}
}