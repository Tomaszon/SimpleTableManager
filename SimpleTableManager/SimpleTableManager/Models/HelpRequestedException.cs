using System.Collections.Generic;

namespace SimpleTableManager.Models
{
	public class HelpRequestedException : CommandException
	{
		public List<string> AvailableKeys { get; set; }

		public CommandReference CommandReference { get; set; }

		public HelpRequestedException(string rawCommand, List<string> availableKeys, CommandReference commandReference) : base(rawCommand, "Help for command")
		{
			AvailableKeys = availableKeys;
			CommandReference = commandReference;
		}
	}
}
