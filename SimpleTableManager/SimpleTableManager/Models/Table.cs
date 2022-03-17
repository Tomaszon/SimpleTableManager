using System.Collections.Generic;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class Table
	{
		public string Name { get; set; }

		public Size Size { get; set; }

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

		public Table(string name, int columnCount, int rowCount)
		{
			Name = name;
			Size = new Size(columnCount, rowCount);
			for (int y = 0; y < rowCount; y++)
			{
				for (int x = 0; x < columnCount; x++)
				{
					Cells.Add(new Cell() { Position = new Position(x, y), Content = $"x:{x},y:{y}" });
				}
			}
		}

		[CommandReference]
		public void AddRowAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

			for (int x = 0; x < Size.Width; x++)
			{
				Cells.Insert(index * Size.Width + x, new Cell() { Position = new Position(x, index), Content = $"x:{x},NEW" });
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
				Cells.Insert(Size.Width * y + y + index, new Cell() { Position = new Position(index, y), Content = $"NEW,y:{y}" });
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
	}
}
