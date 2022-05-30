using System;

namespace SimpleTableManager.Models
{
	public class IncompleteCommandException : Exception
	{
		public string FullValue { get; set; }

		public IncompleteCommandException(string fullValue) : base($"Can not execute incomplete command '{fullValue}'")
		{

		}
	}
}
