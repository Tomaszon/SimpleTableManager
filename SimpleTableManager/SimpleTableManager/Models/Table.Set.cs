using SimpleTableManager.Models.Attributes;
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
		public void SetViewOptions(int x1, int y1, int x2, int y2)
		{
			Shared.Validate(() => x1 >= 0 && x1 <= x2, $"Index x1 is not in the needed range: [0, {x2}]");
			Shared.Validate(() => x2 < Size.Width, $"Index x2 is not in the needed range: [{x1}, {Size.Width - 1}]");

			Shared.Validate(() => y1 >= 0 && y1 <= y2, $"Index y1 is not in the needed range: [0, {y2}]");
			Shared.Validate(() => y2 < Size.Height, $"Index y2 is not in the needed range: [{y1}, {Size.Height - 1}]");

			ViewOptions.StartPosition = new Position(x1, y1);
			ViewOptions.EndPosition = new Position(x2, y2);
		}

		[CommandReference]
		public void SetViewOptionsColumns(int x1, int x2)
		{
			SetViewOptions(x1, ViewOptions.StartPosition.Y, x2, ViewOptions.EndPosition.Y);
		}

		[CommandReference]
		public void SetViewOptionsRows(int y1, int y2)
		{
			SetViewOptions(ViewOptions.StartPosition.X, y1, ViewOptions.EndPosition.X, y2);
		}

		[CommandReference]
		public void SetCellContent(Position position, params object[] contents)
		{
			this[position].SetContent(contents);
		}
	}
}
