namespace SimpleTableManager.Models.Exceptions;

public class HelpRequestedException : CommandException
{
	public List<(string key, bool isLeaf)>? AvailableKeys { get; set; }

	public CommandReference? CommandReference { get; set; }

	public HelpRequestedException(string rawCommand, List<(string, bool)>? availableKeys, CommandReference? commandReference) : base(rawCommand, "Help for command")
	{
		AvailableKeys = availableKeys;
		CommandReference = commandReference;
	}
}