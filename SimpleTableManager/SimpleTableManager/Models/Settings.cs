using System;

namespace SimpleTableManager.Models
{
	public static class Settings
	{
		public static ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.DarkGreen;

		public static ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;

		public static ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGreen;

		public static ConsoleColor SelectedForegroundColor { get; set; } = ConsoleColor.Yellow;

		public static bool ModernTableBorder { get; set; } = false;

		public static int RenderDelay { get; set; } = 100;
	}
}
