namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	private void SelectCell(Cell cell)
	{
		cell.Select();
	}

	[CommandFunction]
	public void SelectCell(Position position, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		SelectCell(this[position]);
	}

	[CommandFunction]
	public void SelectCells([MinLength(1)] params IEnumerable<Position> positions)
	{
		positions.ForEach(p => SelectCell(this[p]));
	}

	[CommandFunction]
	public void SelectCellRange(Position positionFrom, Position positionTo, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		this[positionFrom, positionTo].ForEach(SelectCell);
	}

	[CommandFunction]
	public void SelectColumn(int x, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		ColumnAt(x).ForEach(SelectCell);
	}

	[CommandFunction]
	public void SelectRow(int y, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		RowAt(y).ForEach(SelectCell);
	}

	[CommandFunction, CommandShortcut("selectAllCells")]
	public void SelectAll()
	{
		Content.ForEach(SelectCell);
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionRight()
	{
		var selectedCells = GetPrimarySelectedCells();

		var newPositions = selectedCells.Select(cell =>
		{
			var position = this[cell];

			return new Position((position.X + 1) % Size.Width, position.Y);
		}).ToList();

		DeselectAll();
		SelectCells(newPositions);
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionLeft()
	{
		var selectedCells = GetPrimarySelectedCells();

		var newPositions = selectedCells.Select(cell =>
		{
			var position = this[cell];

			return new Position((position.X - 1 + Size.Width) % Size.Width, position.Y);
		}).ToList();

		DeselectAll();
		SelectCells(newPositions);
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionDown()
	{
		var selectedCells = GetPrimarySelectedCells();

		var newPositions = selectedCells.Select(cell =>
		{
			var position = this[cell];

			return new Position(position.X, (position.Y + 1) % Size.Height);
		}).ToList();

		DeselectAll();
		SelectCells(newPositions);
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionUp()
	{
		var selectedCells = GetPrimarySelectedCells();

		var newPositions = selectedCells.Select(cell =>
		{
			var position = this[cell];

			return new Position(position.X, (position.Y - 1 + Size.Height) % Size.Height);
		}).ToList();

		DeselectAll();
		SelectCells(newPositions);
	}
}