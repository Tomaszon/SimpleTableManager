using System.Collections.Generic;

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
	}
}
