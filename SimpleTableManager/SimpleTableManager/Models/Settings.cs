using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTableManager.Models
{
	public static class Settings
	{
		public static ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;

		public static ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;

		public static ConsoleColor BorderColor { get; set; } = ConsoleColor.Yellow;

		public static bool ModernTableBorder { get; set; } = false;
	}
}
