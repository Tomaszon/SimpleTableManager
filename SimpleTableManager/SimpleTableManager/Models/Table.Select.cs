namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void SelectCell(int x, int y)
		{
			this[x, y].IsSelected = true;
		}

		[CommandReference]
		public void SelectCells(int x1, int y1, int x2, int y2)
		{
			this[x1, y1, x2, y2].ForEach(c => c.IsSelected = true);
		}

		[CommandReference]
		public void SelectColumn(int x)
		{
			Columns[x].ForEach(c => c.IsSelected = true);
		}

		[CommandReference]
		public void SelectRow(int y)
		{
			Rows[y].ForEach(c => c.IsSelected = true);
		}

		[CommandReference]
		public void SelectAll()
		{
			Cells.ForEach(c => c.IsSelected = true);
		}
	}
}
