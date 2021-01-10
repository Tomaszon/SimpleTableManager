using System;

using SimpleTableManager.Models;
using SimpleTableManager.Services;
using Newtonsoft.Json;

namespace SimpleTableManager
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			TableBorderCharacters.FromJson($@".\Configs\TableBorderCharacters.json");

			Settings.ModernTableBorder = false;

			var viewOptions = new ViewOptions(2, 1, 4, 4);

			var table = new Table("", 9, 9);

			foreach (var cell in table.Cells)
			{
				if(cell.Position.X %2 == 0)
				{
					cell.ForegroundColor = ConsoleColor.Black;
					cell.BackgroundColor = ConsoleColor.White;
				}
			}

			Console.WriteLine(JsonConvert.SerializeObject(viewOptions));

			TableRenderer.Render(table, viewOptions);

			Console.ReadKey();
		}
	}
}
