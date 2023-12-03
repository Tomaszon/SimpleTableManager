using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

public partial class Table
{
	[CommandReference]
	public void SelectCell(Position position, bool deselectCurrent = false)
	{
		if(deselectCurrent)
		{
			DeselectAll();
		}

		this[position].IsSelected = true;
	}

	[CommandReference]
	public void SelectCells(params Position[] positions)
	{
		ThrowIfNot<TargetParameterCountException>(positions.Length > 0, "One or more positions needed!");

		positions.ForEach(p => this[p].IsSelected = true);
	}

	[CommandReference]
	public void SelectCellRange(Position positionFrom, Position positionTo, bool deselectCurrent = false)
	{
		if(deselectCurrent)
		{
			DeselectAll();
		}

		this[positionFrom, positionTo].ForEach(c => c.IsSelected = true);
	}

	[CommandReference]
	public void SelectColumn(int x, bool deselectCurrent = false)
	{
		if(deselectCurrent)
		{
			DeselectAll();
		}

		Columns[x].ForEach(c => c.IsSelected = true);
	}

	[CommandReference]
	public void SelectRow(int y, bool deselectCurrent = false)
	{
		if(deselectCurrent)
		{
			DeselectAll();
		}

		Rows[y].ForEach(c => c.IsSelected = true);
	}

	[CommandReference]
	public void SelectAll()
	{
		Content.ForEach(c => c.IsSelected = true);
	}
}