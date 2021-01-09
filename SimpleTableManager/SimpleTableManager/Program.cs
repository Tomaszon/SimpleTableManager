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

			var viewOptions = new ViewOptions(3, 4, 3, 2);

			var table = new Table("", 9, 5);

			Console.WriteLine(JsonConvert.SerializeObject(viewOptions));

			Console.WriteLine(TableRenderer.Render(table, viewOptions));

			Console.ReadKey();
		}
	}
}
