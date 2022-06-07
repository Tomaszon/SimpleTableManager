using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models;
using SimpleTableManager.Services;

namespace SimpleTableManager
{
	public class App
	{
		[CommandReference]
		public void Exit()
		{
			var answer = Shared.ReadLineWhile("Are you sure y/n", new[] { "y", "n" });

			if (answer.ToLower() == "y")
			{
				Console.WriteLine("Good bye!");

				Console.ReadKey();

				Environment.Exit(0);
			}
		}

		[CommandReference("Refreshes the view")]
		public void Refresh()
		{
			Console.ResetColor();
			Console.Clear();
		}
	}
}
