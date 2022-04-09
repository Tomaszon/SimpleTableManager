﻿using System;
using System.IO;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public class Settings
	{
		public ConsoleColorSet TextColor { get; set; } = (ConsoleColor.Gray, ConsoleColor.Black);

		public ConsoleColorSet DefaultContentColor { get; set; } = (ConsoleColor.Gray, ConsoleColor.Black);

		public ConsoleColorSet DefaultBorderColor { get; set; } = (ConsoleColor.Gray, ConsoleColor.Black);

		public ConsoleColorSet SelectedContentColor { get; set; } = (ConsoleColor.Yellow, ConsoleColor.Black);

		public ConsoleColorSet SelectedBorderColor { get; set; } = (ConsoleColor.Yellow, ConsoleColor.Black);

		public bool ModernTableBorder { get; set; } = false;

		public static void FromJson(string path)
		{
			Current = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
		}

		public static Settings Current { get; private set; }
	}
}