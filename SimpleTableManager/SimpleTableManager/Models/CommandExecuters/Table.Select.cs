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

		this[position].SelectionLevel |= CellSelectionLevel.Primary;
	}

	[CommandFunction]
	public void SelectCells(params Position[] positions)
	{
		//TODO test MinLength attribute
		positions.ForEach(p => this[p].SelectionLevel |= CellSelectionLevel.Primary);
	}

	[CommandFunction]
	public void SelectCellRange(Position positionFrom, Position positionTo, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		this[positionFrom, positionTo].ForEach(c => c.SelectionLevel |= CellSelectionLevel.Primary);
	}

	[CommandFunction]
	public void SelectColumn(int x, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		Columns[x].ForEach(c => c.SelectionLevel |= CellSelectionLevel.Primary);
	}

	[CommandFunction]
	public void SelectRow(int y, bool deselectCurrent = false)
	{
		if (deselectCurrent)
		{
			DeselectAll();
		}

		Rows[y].ForEach(c => c.SelectionLevel |= CellSelectionLevel.Primary);
	}

	[CommandFunction, CommandShortcut("selectAllCells")]
	public void SelectAll()
	{
		Content.ForEach(c => c.SelectionLevel |= CellSelectionLevel.Primary);
	}

	[CommandFunction, CommandShortcut]
	public void MoveSelectionRight()
	{
		var selectedCells = GetPrimarySelectedCells();

		var newPositions = selectedCells.Select(cell =>
		{
			var position = this[cell];

			return new Position((position.X + 1) % Size.Width, position.Y);
		}).ToArray();

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
		}).ToArray();

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
		}).ToArray();

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
		}).ToArray();

		DeselectAll();
		SelectCells(newPositions);
	}
}