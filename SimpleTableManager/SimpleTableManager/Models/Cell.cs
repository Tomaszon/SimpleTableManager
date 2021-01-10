using System;

namespace SimpleTableManager.Models
{
	public class Cell
	{
		public Position Position { get; set; }

		public Size Size { get; set; } = new Size(7, 1);

		public object Content { get; set; }

		public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

		public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
	}
}
