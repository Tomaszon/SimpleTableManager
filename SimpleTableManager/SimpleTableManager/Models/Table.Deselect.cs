using System.Reflection;
using SimpleTableManager.Extensions;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void DeselectCell(int x, int y)
		{
			this[x, y].IsSelected = false;
		}

		[CommandReference]
		public void DeselectCells(params Position[] positions)
		{
			Shared.Validate<TargetParameterCountException>(() => positions.Length > 0, "One or more positions needed!");

			positions.ForEach(p => this[p].IsSelected = false);
		}

		[CommandReference]
		public void DeselectCellRange(int x1, int y1, int x2, int y2)
		{
			this[x1, y1, x2, y2].ForEach(c => c.IsSelected = false);
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
