using System;
using SimpleTableManager.Models.Attributes;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void ResetViewOptions()
		{
			ViewOptions = new ViewOptions(0, 0, Math.Max(Size.Width - 1, 0), Math.Max(Size.Height - 1, 0));
		}

		[CommandReference]
		public void HideColumn(int x)
		{
			Header[x].IsHidden = true;
			Columns[x].ForEach(c => c.IsHidden = true);
		}

		[CommandReference]
		public void HideRow(int y)
		{
			Sider[y].IsHidden = true;
			Rows[y].ForEach(c => c.IsHidden = true);
		}

		[CommandReference]
		public void ShowColumn(int x)
		{
			Header[x].IsHidden = false;
			Columns[x].ForEach(c => c.IsHidden = false);
		}

		[CommandReference]
		public void ShowRow(int y)
		{
			Sider[y].IsHidden = false;
			Rows[y].ForEach(c => c.IsHidden = false);
		}

		[CommandReference]
		public object ShowCellDetails(Position position)
		{
			return this[position].ShowDetails();
		}

		[CommandReference]
		public object ShowCellContentFunction(Position position)
		{
			return this[position].ShowContentFunction();
		}
	}
}