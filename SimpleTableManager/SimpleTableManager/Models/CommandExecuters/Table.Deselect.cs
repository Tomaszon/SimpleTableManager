namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	public void DeselectCell(Cell cell)
	{
		cell.Deselect();
	}

	[CommandFunction]
	public void DeselectCell(Position position)
	{
		DeselectCell(this[position]);
	}

	[CommandFunction]
	public void DeselectCells(params Position[] positions)
	{
		ThrowIfNot<TargetParameterCountException>(positions.Length > 0, "One or more positions needed!");

		positions.ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectCellRange(Position positionFrom, Position positionTo)
	{
		this[positionFrom, positionTo].ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectColumn(int x)
	{
		ColumnAt(x).ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectRow(int y)
	{
		RowAt(y).ForEach(DeselectCell);
	}

	[CommandFunction, CommandShortcut("deselectAllCells")]
	public void DeselectAll()
	{
		Content.ForEach(DeselectCell);
	}
}