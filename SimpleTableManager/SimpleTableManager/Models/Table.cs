using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		public string Name { get; set; }

		public Size Size { get; set; }

		public bool IsActive { get; set; }

		public ViewOptions ViewOptions { get; set; }

		public List<Cell> Cells { get; set; } = new List<Cell>();

		public Cell this[int x, int y]
		{
			get
			{
				return Cells[y * Size.Width + x];
			}
		}

		public List<Cell> this[int x1, int y1, int x2, int y2]
		{
			get
			{
				List<Cell> result = new List<Cell>();

				for (int y = y1; y <= y2; y++)
				{
					for (int x = x1; x <= x2; x++)
					{
						result.Add(this[x, y]);
					}
				}

				return result;
			}
		}

		public List<Cell> GetColumn(int x)
		{
			return this[x, 0, x, Size.Height - 1];
		}

		public List<Cell> GetRow(int y)
		{
			return this[0, y, Size.Width - 1, y];
		}

		public Table(string name, int columnCount, int rowCount)
		{
			Name = name;
			Size = new Size(columnCount, rowCount);
			ResetViewOptions();

			for (int y = 0; y < rowCount; y++)
			{
				for (int x = 0; x < columnCount; x++)
				{
					Cells.Add(new Cell() { BackgroundColor = Settings.DefaultBackgroundColor, ForegroundColor = Settings.DefaultForegroundColor });
				}
			}
		}

		public List<int> GetColumnWidths()
		{
			return Shared.IndexArray(Size.Width).Select(i => GetColumnWidth(i)).ToList();
		}

		public List<int> GetRowHeights()
		{
			return Shared.IndexArray(Size.Height).Select(i => GetRowHeight(i)).ToList();
		}

		public int GetRowHeight(int index)
		{
			return this[0, index, Size.Width - 1, index].Max(c => c.Size.Height);
		}

		public int GetColumnWidth(int index)
		{
			return this[index, 0, index, Size.Height - 1].Max(c => c.Size.Width);
		}


		#region Set
		[CommandReference]
		public void SetColumnWidth(int index, int width)
		{
			this[index, 0, index, Size.Width - 1].ForEach(c => c.GivenSize = new Size(width, c.GivenSize.Height));
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
		public void ResetViewOptions()
		{
			ViewOptions = new ViewOptions(0, 0, Size.Width - 1, Size.Height - 1);
		}

		#endregion

		public IEnumerable<Cell> GetSelectedCells()
		{
			return Cells.Where(c => c.IsSelected);
		}
	}
}
