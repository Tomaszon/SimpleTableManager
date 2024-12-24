namespace SimpleTableManager.Models.Exceptions;

public class HelpRequestedException(string rawCommand, List<(string, bool)>? availableKeys, CommandReference? commandReference) : CommandException(rawCommand, "Help for command")
{
	public List<(string key, bool isLeaf)>? AvailableKeys { get; set; } = availableKeys;

	public CommandReference? CommandReference { get; set; } = commandReference;
}