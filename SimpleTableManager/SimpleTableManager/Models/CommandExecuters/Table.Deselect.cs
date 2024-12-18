namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void DeselectCell(Position position)
	{
		this[position].SelectionLevel = CellSelectionLevel.None;
	}

	[CommandFunction]
	public void DeselectCells(params Position[] positions)
	{
		ThrowIfNot<TargetParameterCountException>(positions.Length > 0, "One or more positions needed!");

		positions.ForEach(p => this[p].SelectionLevel = CellSelectionLevel.None);
	}

	[CommandFunction]
	public void DeselectCellRange(Position positionFrom, Position positionTo)
	{
		this[positionFrom, positionTo].ForEach(c => c.SelectionLevel = CellSelectionLevel.None);
	}

	[CommandFunction]
	public void DeselectColumn(int x)
	{
		Columns[x].ForEach(c => c.SelectionLevel = CellSelectionLevel.None);
	}

	[CommandFunction]
	public void DeselectRow(int y)
	{
		Rows[y].ForEach(c => c.SelectionLevel = CellSelectionLevel.None);
	}

	[CommandFunction, CommandShortcut("deselectAllCells")]
	public void DeselectAll()
	{
		Content.ForEach(c => c.SelectionLevel = CellSelectionLevel.None);
	}
}