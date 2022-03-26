using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class Table
	{
		public string Name { get; set; }

		public Size Size { get; set; }

		public List<Cell> Cells { get; set; } = new List<Cell>();

		public List<Cell> this[int index, SelectType selectType]
		{
			get
			{
				return selectType switch
				{
					SelectType.Row => this[0, index, Size.Width - 1, index],
					SelectType.Column => this[index, 0, index, Size.Height - 1],

					_ => throw new System.NotImplementedException()
				};
			}
		}

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

		public Table(string name, int columnCount, int rowCount)
		{
			Name = name;
			Size = new Size(columnCount, rowCount);
			for (int y = 0; y < rowCount; y++)
			{
				for (int x = 0; x < columnCount; x++)
				{
					Cells.Add(new Cell("T", $"x:{x}", $"y:{y}") { BackgroundColor = Settings.DefaultBackgroundColor, ForegroundColor = Settings.DefaultForegroundColor });
				}
			}
		}

		public int GetRowHeight(int index)
		{
			return this[0, index, Size.Width - 1, index].Max(c => c.Size.Height);
		}

		public int GetColumnWidth(int index)
		{
			return this[index, 0, index, Size.Height - 1].Max(c => c.Size.Width);
		}

		#region Add
		[CommandReference]
		public void AddRowAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

			for (int x = 0; x < Size.Width; x++)
			{
				Cells.Insert(index * Size.Width + x, new Cell($"N:{x},{index}") { BackgroundColor = Settings.DefaultBackgroundColor, ForegroundColor = Settings.DefaultForegroundColor });
			}

			Size.Height++;
		}

		[CommandReference]
		public void AddRowAfter(int after)
		{
			Shared.Validate(() => after >= 0 && after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

			AddRowAt(after + 1);
		}

		[CommandReference]
		public void AddRowFirst()
		{
			AddRowAt(0);
		}

		[CommandReference]
		public void AddRowLast()
		{
			AddRowAt(Size.Height);
		}

		[CommandReference]
		public void AddColumnAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

			for (int y = 0; y < Size.Height; y++)
			{
				Cells.Insert(Size.Width * y + y + index, new Cell($"NEW,y:{y}") { BackgroundColor = Settings.DefaultBackgroundColor, ForegroundColor = Settings.DefaultForegroundColor });
			}

			Size.Width++;
		}

		[CommandReference]
		public void AddColumnAfter(int after)
		{
			Shared.Validate(() => after >= 0 && after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

			AddColumnAt(after + 1);
		}

		[CommandReference]
		public void AddColumnFirst()
		{
			AddColumnAt(0);
		}

		[CommandReference]
		public void AddColumnLast()
		{
			AddColumnAt(Size.Width);
		}

		[CommandReference]
		public void AddCellContent(string content)
		{
			Cells.Where(c => c.IsSelected).ForEach(c => c.AddContent(content));
		}
		#endregion

		#region Remove|Delete
		[CommandReference]
		public void RemoveRowAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Height - 1, $"Index is not in the needed range: [0, {Size.Height - 1}]");

			Cells.RemoveRange(index * Size.Width, Size.Width);

			Size.Height--;
		}

		[CommandReference]
		public void RemoveFirstRow()
		{
			RemoveRowAt(0);
		}

		[CommandReference]
		public void RemoveLastRow()
		{
			RemoveRowAt(Size.Height - 1);
		}

		[CommandReference]
		public void RemoveColumnAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Width - 1, $"Index is not in the needed range: [0, {Size.Width - 1}]");

			for (int y = 0; y < Size.Height; y++)
			{
				Cells.RemoveAt(Size.Width * y - y + index);
			}

			Size.Width--;
		}

		[CommandReference]
		public void RemoveFirstColumn()
		{
			RemoveColumnAt(0);
		}

		[CommandReference]
		public void RemoveLastColumn()
		{
			RemoveColumnAt(Size.Width - 1);
		}

		[CommandReference]
		public void RemoveCellContent()
		{
			Cells.Where(c => c.IsSelected).ForEach(c => c.RemoveContent());
		}

		#endregion

		#region Set
		[CommandReference]
		public void SetColumnWidth(int index, int width)
		{
			this[index, 0, index, Size.Width - 1].ForEach(c => c.GivenSize = new Size(width, c.GivenSize.Height));
		}

		[CommandReference]
		public void SetCellContent(string[] content)
		{
			Cells.Where(c => c.IsSelected).ForEach(c => c.SetContent(content));
		}
		#endregion



		#region Select
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
		#endregion

		#region Deselect
		[CommandReference]
		public void DeselectCell(int x, int y)
		{
			this[x, y].IsSelected = false;
		}

		[CommandReference]
		public void DeselectCells(int x1, int y1, int x2, int y2)
		{
			this[x1, y1, x2, y2].ForEach(c => c.IsSelected = false);

		}
		#endregion
	}

	public enum SelectType
	{
		Row,
		Column
	}
}
