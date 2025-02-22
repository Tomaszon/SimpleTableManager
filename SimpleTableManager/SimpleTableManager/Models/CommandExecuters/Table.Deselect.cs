namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	public void DeselectCell(Cell cell)
	{
		cell.Deselect();
	}

	public void DeselectCells(IEnumerable<Cell> cells)
	{
		cells.ForEach(c => c.Deselect());
	}

	[CommandFunction]
	public void DeselectCell(Position position)
	{
		DeselectCell(this[position]);
	}

	[CommandFunction]
	public void DeselectCells([MinLength(1)] params IEnumerable<Position> positions)
	{
		positions.ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectCellRange(Position positionFrom, Position positionTo)
	{
		this[positionFrom, positionTo].ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectColumn([MinValue(0)] int x)
	{
		ColumnAt(x).ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectRow([MinValue(0)] int y)
	{
		RowAt(y).ForEach(DeselectCell);
	}

	[CommandFunction, CommandShortcut("deselectAllCells")]
	public void DeselectAll()
	{
		Content.ForEach(DeselectCell);
	}
}