﻿namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void DeselectCell(Position position)
	{
		this[position].Deselect();
	}

	[CommandFunction]
	public void DeselectCells([MinLength(1)] params IEnumerable<Position> positions)
	{
		positions.ForEach(DeselectCell);
	}

	[CommandFunction]
	public void DeselectCellRange(Position positionFrom, Position positionTo)
	{
		this[positionFrom, positionTo].ForEach(c => c.Deselect());
	}

	[CommandFunction]
	public void DeselectColumn([MinValue(0)] int x)
	{
		ColumnAt(x).ForEach(c => c.Deselect());
	}

	[CommandFunction]
	public void DeselectRow([MinValue(0)] int y)
	{
		RowAt(y).ForEach(c => c.Deselect());
	}

	[CommandFunction, CommandShortcut("deselectAllCells")]
	public void DeselectAll()
	{
		Content.ForEach(c => c.Deselect());
	}
}