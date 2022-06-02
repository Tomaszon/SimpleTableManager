using System.Reflection;
using SimpleTableManager.Extensions;
using SimpleTableManager.Services;

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
		public void SelectCells(params Position[] positions)
		{
			Shared.Validate<TargetParameterCountException>(() => positions.Length > 0, "One or more positions needed!");

			positions.ForEach(p => this[p].IsSelected = true);
		}

		[CommandReference]
		public void SelectCellRange(int x1, int y1, int x2, int y2)
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
			Content.ForEach(c => c.IsSelected = true);
		}
	}
}
