using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SimpleTableManager.Extensions;
using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	[CommandInformation("Cell selection and table related commands")]
	public partial class Table
	{
		public string Name { get; set; }

		public Size Size { get; set; } = new Size(0, 0);

		public bool IsActive { get; set; }

		public ViewOptions ViewOptions { get; set; } = new ViewOptions(0, 0, 0, 0);

		public List<Cell> Content { get; set; } = new List<Cell>();

		public List<IndexCell> Header { get; set; } = new List<IndexCell>();

		public List<IndexCell> Sider { get; set; } = new List<IndexCell>();

		public Cell CornerCell { get; set; } = new Cell(@"y \ x");

		public Dictionary<int, List<Cell>> Columns =>
			Shared.IndexArray(Size.Width).ToDictionary(x => x, x => this[x, 0, x, Size.Height - 1]);

		public Dictionary<int, List<Cell>> Rows =>
			Shared.IndexArray(Size.Height).ToDictionary(y => y, y => this[0, y, Size.Width - 1, y]);

		public Cell this[Position position] => this[position.X, position.Y];

		public Cell this[int x, int y] => Content[y * Size.Width + x];

		public Position this[Cell cell]
		{
			get
			{
				var index = Content.IndexOf(cell);

				return new Position(index % Size.Width, index / Size.Width);
			}
		}

		public List<Cell> this[int x1, int y1, int x2, int y2] =>
			Shared.IndexArray(y2 - y1 + 1, y1).SelectMany(y =>
				Shared.IndexArray(x2 - x1 + 1, x1).Select(x => this[x, y])).ToList();


		public Table(string name, int columnCount, int rowCount)
		{
			Name = name;

			Shared.IndexArray(columnCount).ForEach(x => AddColumnLast());
			Shared.IndexArray(rowCount).ForEach(y => AddRowLast());

			ResetViewOptions();
		}

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

		public IndexCell GetFirstVisibleHeaderInView()
		{
			return Header.FirstOrDefault(h => IsColumnInView(h.Index) && !IsColumnHidden(h.Index));
		}

		public IndexCell GetLastVisibleHeaderInView()
		{
			return Header.LastOrDefault(h => IsColumnInView(h.Index) && !IsColumnHidden(h.Index));
		}

		public IndexCell GetFirstVisibleSiderInView()
		{
			return Sider.FirstOrDefault(s => IsRowInView(s.Index) && !IsRowHidden(s.Index));
		}

		public IndexCell GetLastVisibleSiderInView()
		{
			return Sider.LastOrDefault(s => IsRowInView(s.Index) && !IsRowHidden(s.Index));
		}

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

		public Position PositionInView(int x = -1, int y = -1)
		{
			return new Position(x - ViewOptions.StartPosition.X, y - ViewOptions.StartPosition.Y);
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

		public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is Cell cell)
			{
				switch (e.PropertyName)
				{
					case nameof(Cell.ContentFunction):
						{
							if (cell.ContentFunction is not null)
							{
								UpdateReferenceCellContent(cell);
							}
						}
						break;

					case nameof(Cell.Content):
						{
							var position = this[cell];

							var referenceCells = Content.Where(c =>
								c.ContentFunction?.GetReferredCellPositions().Contains(position) == true);

							referenceCells.ForEach(c => UpdateReferenceCellContent(c));
						}
						break;
				}
			}
		}

		private void UpdateReferenceCellContent(Cell cell)
		{
			var referredPositions = cell.ContentFunction.GetReferredCellPositions();

			if (referredPositions.Contains(this[cell]))
			{
				cell.ContentType = typeof(string);
				cell.SetContent("Ref error");
			}
			else
			{
				//TODO do not use only the first item in Content
				var arguments = referredPositions.Select(p =>
					new FunctionParameter(this[p].Content.FirstOrDefault(), p)).Where(p => p.Value is not null).ToArray();

				cell.ContentFunction.Arguments.RemoveAll(a =>
					a.IsReference && !arguments.Any(p =>
						p.ReferencePosition.Equals(a.ReferencePosition)));
						
				var result = cell.ContentFunction.Execute(arguments).Value;

				cell.ContentType = result.GetType();
				cell.SetContent(result);
			}
		}
	}
}
