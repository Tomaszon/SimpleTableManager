using System;

namespace SimpleTableManager.Models
{
	public class IncompleteCommandException : Exception
	{
		public string FullValue { get; set; }

		public IncompleteCommandException(string fullValue) : base($"Incomplete command '{fullValue}'")
		{

		}
	}
}
