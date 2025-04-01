namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	public void SelectCells(IEnumerable<Cell> cells)
	{
		cells.ForEach(c => c.Select());
	}

	[CommandFunction]
	public void SelectCell(Position position, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAllCore();
		}

		this[position].Select();
	}

	[CommandFunction]
	public void SelectCells([MinLength(1)] params IEnumerable<Position> positions)
	{
		positions.ForEach(p => this[p].Select());
	}

	[CommandFunction]
	public void SelectCellRange(Position positionFrom, Position positionTo, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAllCore();
		}

		this[positionFrom, positionTo].ForEach(c => c.Select());
	}

	[CommandFunction]
	public void SelectColumn([MinValue(0)] int x, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAllCore();
		}

		ColumnAt(x).ForEach(c => c.Select());
	}

	[CommandFunction]
	public void SelectRow([MinValue(0)] int y, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAllCore();
		}

		RowAt(y).ForEach(c => c.Select());
	}

	[CommandFunction, CommandShortcut("selectAllCells")]
	public void SelectAll()
	{
		Content.ForEach(c => c.Select());
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

		DeselectAllCore();
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

		DeselectAllCore();
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

		DeselectAllCore();
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

		DeselectAllCore();
		SelectCells(newPositions);
	}
}