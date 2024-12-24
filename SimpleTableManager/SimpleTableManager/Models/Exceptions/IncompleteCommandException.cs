namespace SimpleTableManager.Models.Exceptions;

public class IncompleteCommandException(string rawCommand, List<string>? availableKeys) : CommandException(rawCommand, $"Can not execute incomplete command '{rawCommand}'")
{
	public List<string>? AvailableKeys { get; set; } = availableKeys;
}