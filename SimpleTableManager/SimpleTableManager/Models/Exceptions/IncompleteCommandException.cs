namespace SimpleTableManager.Models.Exceptions;

public class IncompleteCommandException : CommandException
{
	public List<string>? AvailableKeys { get; set; }

	public IncompleteCommandException(string rawCommand, List<string>? availableKeys) : base(rawCommand, $"Can not execute incomplete command '{rawCommand}'")
	{
		AvailableKeys = availableKeys;
	}
}