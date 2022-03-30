using System;
using System.IO;

namespace SimpleTableManager.Models
{
	public class Settings
	{
		public ConsoleColor TextForegroundColor { get; set; } = ConsoleColor.Gray;

		public ConsoleColor TextBackgroundColor { get; set; } = ConsoleColor.Black;

		public ConsoleColor DefaultCellForegroundColor { get; set; } = ConsoleColor.Gray;

		public ConsoleColor DefaultCellBackgroundColor { get; set; } = ConsoleColor.Black;

		public ConsoleColor DefaultBorderForegroundColor { get; set; } = ConsoleColor.Gray;

		public ConsoleColor DefaultBorderBackgroundColor { get; set; } = ConsoleColor.Black;

		public ConsoleColor SelectedCellForegroundColor { get; set; } = ConsoleColor.Yellow;

		public ConsoleColor SelectedCellBackgroundColor { get; set; } = ConsoleColor.Black;

		public ConsoleColor SelectedBorderForegroundColor { get; set; } = ConsoleColor.Yellow;

		public ConsoleColor SelectedBorderBackgroundColor { get; set; } = ConsoleColor.Black;

		public bool ModernTableBorder { get; set; } = false;

		public static void FromJson(string path)
		{
			Current = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
		}

		public static Settings Current { get; private set; }
	}
}
