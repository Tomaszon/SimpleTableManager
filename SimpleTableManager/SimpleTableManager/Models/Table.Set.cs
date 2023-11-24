using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void SetName(string name)
		{
			Name = name;
		}

		[CommandReference]
		public void SetSize(Size size)
		{
			Shared.Validate(() => size.Width > 0 && size.Height > 0, "Can not decrease table size under 1 column or 1 row!");

			while (Size.Width != size.Width)
			{
				if (Size.Width > size.Width)
				{
					RemoveLastColumn();
				}
				else
				{
					AddColumnLast();
				}
			}
			while (Size.Height != size.Height)
			{
				if (Size.Height > size.Height)
				{
					RemoveLastRow();
				}
				else
				{
					AddRowLast();
				}
			}
		}

		[CommandReference]
		public void SetColumnWidth(int index, int width)
		{
			Columns[index].ForEach(c => c.GivenSize = new Size(width, c.GivenSize.Height));
		}

		[CommandReference]
		public void SetRowHeight(int index, int height)
		{
			Rows[index].ForEach(c => c.GivenSize = new Size(c.GivenSize.Width, height));
		}

		[CommandReference]
		public void SetViewOptions(Position from, Position to)
		{
			Shared.Validate(() => from.X >= 0 && from.X <= to.X, $"Index x1 is not in the needed range: [0, {to.X}]");
			Shared.Validate(() => to.X < Size.Width, $"Index x2 is not in the needed range: [{from.X}, {Size.Width - 1}]");

			Shared.Validate(() => from.Y >= 0 && from.Y <= to.Y, $"Index y1 is not in the needed range: [0, {to.Y}]");
			Shared.Validate(() => to.Y < Size.Height, $"Index y2 is not in the needed range: [{from.Y}, {Size.Height - 1}]");

			ViewOptions.StartPosition = from;
			ViewOptions.EndPosition = to;
		}

		[CommandReference]
		public void SetViewOptionsColumns(int x1, int x2)
		{
			SetViewOptions(new Position(x1, ViewOptions.StartPosition.Y), new Position(x2, ViewOptions.EndPosition.Y));
		}

		[CommandReference]
		public void SetViewOptionsRows(int y1, int y2)
		{
			SetViewOptions(new Position(ViewOptions.StartPosition.X, y1), new Position(ViewOptions.EndPosition.X, y2));
		}

		[CommandReference]
		public void SetCellContent(Position position, params string[] contents)
		{
			this[position].SetContent(contents);
		}

		// [CommandReference]
		// public void SetCellType(Position position, string typeName)
		// {
		// 	this[position].SetType(typeName);
		// }
	}
}
