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

		public List<Cell> Content { get; set; } = new List<Cell>();

		public List<Cell> Header { get; set; } = new List<Cell>();

		public List<Cell> Sider { get; set; } = new List<Cell>();

		public Cell CornerCell { get; set; } = new Cell(@"y \ x")
		{
			//BackgroundColor = System.ConsoleColor.DarkGray,
			//ForegroundColor = System.ConsoleColor.Black,
			//BorderBackgroundColor = System.ConsoleColor.DarkGray,
			//BorderForegroundColor = System.ConsoleColor.Black
		};

		public Dictionary<int, List<Cell>> Columns
		{
			get => Shared.IndexArray(Size.Width).ToDictionary(x => x, x => this[x, 0, x, Size.Height - 1]);
		}

		public Dictionary<int, List<Cell>> Rows
		{
			get => Shared.IndexArray(Size.Height).ToDictionary(y => y, y => this[0, y, Size.Width - 1, y]);
		}

		public Cell this[int x, int y]
		{
			get
			{
				return Content[y * Size.Width + x];
			}
		}

		public List<Cell> this[int x1, int y1, int x2, int y2]
		{
			get
			{
				return Shared.IndexArray(y2 - y1 + 1, y1).SelectMany(y =>
					Shared.IndexArray(x2 - x1 + 1, x1).Select(x => this[x, y])).ToList();
			}
		}

		public Table(string name, int columnCount, int rowCount)
		{
			Name = name;
			Size = new Size(columnCount, rowCount);
			ResetViewOptions();

			Shared.IndexArray(rowCount).ForEach(y =>
				Shared.IndexArray(columnCount).ForEach(x => Content.Add(new Cell("T", new Position(x, y), ":)"))));

			Shared.IndexArray(columnCount).ForEach(x => Header.Add(new Cell(x)
			{
				ContentType = typeof(int),
				//BackgroundColor = System.ConsoleColor.DarkGray,
				//ForegroundColor = System.ConsoleColor.Black,
				//BorderBackgroundColor = System.ConsoleColor.DarkGray,
				//BorderForegroundColor = System.ConsoleColor.Black
			}));

			Shared.IndexArray(rowCount).ForEach(y => Sider.Add(new Cell(y)
			{
				ContentType = typeof(int),
				//BackgroundColor = System.ConsoleColor.DarkGray,
				//ForegroundColor = System.ConsoleColor.Black,
				//BorderBackgroundColor = System.ConsoleColor.DarkGray,
				//BorderForegroundColor = System.ConsoleColor.Black
			}));

			//HideColumn(3);
		}

		//public List<int> GetColumnWidths()
		//{
		//	return Shared.IndexArray(Size.Width).Select(i => GetColumnWidth(i)).ToList();
		//}

		//public List<int> GetRowHeights()
		//{
		//	return Shared.IndexArray(Size.Height).Select(i => GetRowHeight(i)).ToList();
		//}

		public int GetRowHeight(int index)
		{
			return Shared.Max(Rows[index].Max(c => c.Size.Height), Sider[index].Size.Height);
		}

		public int GetColumnWidth(int index)
		{
			return Shared.Max(Columns[index].Max(c => c.Size.Width), Header[index].Size.Width);
		}

		public int GetSiderWidth()
		{
			return Shared.Max(Sider.Max(c => c.Size.Width), CornerCell.Size.Width);
		}

		public int GetHeaderHeight()
		{
			return Shared.Max(Header.Max(c => c.Size.Height), CornerCell.Size.Height);
		}

		public Size GetTableSize()
		{
			var sumWidth = Shared.IndexArray(Size.Width).Where(x => !IsColumnHidden(x) && IsColumnInView(x)).Sum(x => GetColumnWidth(x) - 1);
			var sumHeight = Shared.IndexArray(Size.Height).Where(y => !IsRowHidden(y) && IsRowInView(y)).Sum(y => GetRowHeight(y) - 1);

			return new Size(GetSiderWidth() + sumWidth, GetHeaderHeight() + sumHeight);
		}

		public Size GetContentCellSize(int x, int y)
		{
			return new Size(GetColumnWidth(x), GetRowHeight(y));
		}

		public Size GetHeaderCellSize(int x)
		{
			return new Size(GetColumnWidth(x), GetHeaderHeight());
		}

		public Size GetSiderCellSize(int y)
		{
			return new Size(GetSiderWidth(), GetRowHeight(y));
		}

		public Size GetCornerCellSize()
		{
			return new Size(GetSiderWidth(), GetHeaderHeight());
		}

		public Position GetHeaderCellPosition(Size tableOffset, int x)
		{
			var sumWidth = x > 0 ? Shared.IndexArray(x).Where(x => !IsColumnHidden(x) && IsColumnInView(x)).Sum(x => GetColumnWidth(x) - 1) : 0;

			return new Position(tableOffset.Width + GetSiderWidth() + sumWidth - 1, tableOffset.Height);
		}

		public Position GetSiderCellPosition(Size tableOffset, int y)
		{
			var sumHeight = y > 0 ? Shared.IndexArray(y).Where(y => !IsRowHidden(y) && IsRowInView(y)).Sum(y => GetRowHeight(y) - 1) : 0;

			return new Position(tableOffset.Width, tableOffset.Height + GetHeaderHeight() + sumHeight - 1);
		}

		public Position GetContentCellPosition(Size tableOffset, int x, int y)
		{
			var sumWidth = x > 0 ? Shared.IndexArray(x).Where(x => !IsColumnHidden(x) && IsColumnInView(x)).Sum(x => GetColumnWidth(x) - 1) : 0;

			var sumHeight = y > 0 ? Shared.IndexArray(y).Where(y => !IsRowHidden(y) && IsRowInView(y)).Sum(y => GetRowHeight(y) - 1) : 0;

			return new Position(tableOffset.Width + GetSiderWidth() + sumWidth - 1, tableOffset.Height + GetHeaderHeight() + sumHeight - 1);
		}

		public bool IsColumnHidden(int x)
		{
			return Columns[x].All(c => c.IsHidden);
		}

		public bool IsRowHidden(int y)
		{
			return Rows[y].All(c => c.IsHidden);
		}

		public bool IsColumnSelected(int index)
		{
			return index >= 0 && index < Size.Width && Columns[index].Any(c => c.IsSelected);
		}

		public bool IsRowSelected(int index)
		{
			return index >= 0 && index < Size.Height && Rows[index].Any(c => c.IsSelected);
		}

		public bool IsCellSelected(int x, int y)
		{
			return x > 0 && x < Size.Width && y > 0 && y < Size.Height && this[x, y].IsSelected;
		}

		//#region Set
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

		//[CommandReference]
		//public void SetViewOptionsColumns(int x1, int x2)
		//{
		//	SetViewOptions(x1, ViewOptions.StartPosition.Y, x2, ViewOptions.EndPosition.Y);
		//}

		//[CommandReference]
		//public void SetViewOptionsRows(int y1, int y2)
		//{
		//	SetViewOptions(ViewOptions.StartPosition.X, y1, ViewOptions.EndPosition.X, y2);
		//}

		[CommandReference]
		public void ResetViewOptions()
		{
			ViewOptions = new ViewOptions(0, 0, Size.Width - 1, Size.Height - 1);
		}
		//#endregion

		public IEnumerable<Cell> GetSelectedCells()
		{
			return Content.Where(c => c.IsSelected);
		}

		public bool IsCellInView(int x = -1, int y = -1)
		{
			x = x == -1 ? ViewOptions.StartPosition.X : x;
			y = y == -1 ? ViewOptions.StartPosition.Y : y;

			return IsColumnInView(x) && IsRowInView(y);
		}

		public (int vX, int vY) PositionInView(int x, int y)
		{
			return (x - ViewOptions.StartPosition.X, y - ViewOptions.StartPosition.Y);
		}

		public bool IsColumnInView(int x)
		{
			return x >= ViewOptions.StartPosition.X && x <= ViewOptions.EndPosition.X;
		}

		public bool IsRowInView(int y)
		{
			return y >= ViewOptions.StartPosition.Y && y <= ViewOptions.EndPosition.Y;
		}

		//public List<Cell> GetCellsInView(int y)
		//{
		//	var siderCell = this[0, y];

		//	var contentCells = Rows[y].GetRange(ViewOptions.StartPosition.X, ViewOptions.Size.Width);

		//	return Enumerable.Union(new[] { siderCell }, contentCells).ToList();
		//}

		public void HideColumn(int x)
		{
			Header[x].IsHidden = true;
			Columns[x].ForEach(c => c.IsHidden = true);
		}

		public void HideRow(int y)
		{
			Sider[y].IsHidden = true;
			Rows[y].ForEach(c => c.IsHidden = true);
		}

		public void ShowColumn(int x)
		{
			Header[x].IsHidden = false;
			Columns[x].ForEach(c => c.IsHidden = false);
		}

		public void ShowRow(int y)
		{
			Sider[y].IsHidden = false;
			Rows[y].ForEach(c => c.IsHidden = false);
		}
	}
}
