using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void DeselectCell(Position position)
		{
			this[position].IsSelected = false;
		}

		[CommandReference]
		public void DeselectCells(params Position[] positions)
		{
			Shared.Validate<TargetParameterCountException>(() => positions.Length > 0, "One or more positions needed!");

			positions.ForEach(p => this[p].IsSelected = false);
		}

		[CommandReference]
		public void DeselectCellRange(Position positionFrom, Position positionTo)
		{
			this[positionFrom, positionTo].ForEach(c => c.IsSelected = false);
		}

		[CommandReference]
		public void DeselectColumn(int x)
		{
			Columns[x].ForEach(c => c.IsSelected = false);
		}

		[CommandReference]
		public void DeselectRow(int y)
		{
			Rows[y].ForEach(c => c.IsSelected = false);
		}

		[CommandReference]
		public void DeselectAll()
		{
			Content.ForEach(c => c.IsSelected = false);
		}
	}
}
