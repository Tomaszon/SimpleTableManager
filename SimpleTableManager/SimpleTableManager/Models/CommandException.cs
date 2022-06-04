using System;

namespace SimpleTableManager.Models
{
	public class CommandException : Exception
	{
		public string RawCommand { get; set; }

		public CommandException(string rawCommand, string message) : base(message)
		{
			RawCommand = rawCommand;
		}
	}
}
