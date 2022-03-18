using System;
using System.Linq;

namespace SimpleTableManager.Models
{
	public class Cell
	{
		public Size ContentSize => Content is { } ?
			new Size(Content.ToString().Length + 2, Content.ToString().ToCharArray().Count(c => c == '\n') + 1) : new Size(0, 1);

		public Size GivenSize { get; set; } = new Size(0, 0);

		public Size Size =>
			new Size(Math.Max(ContentSize.Width, GivenSize.Width), Math.Max(ContentSize.Height, GivenSize.Height));

		public object Content { get; set; }

		public bool IsSelected { get; set; }

		public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

		public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

		public Cell(object content)
		{
			Content = content;
		}
	}
}
