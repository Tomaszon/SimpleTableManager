namespace SimpleTableManager.Models.Exceptions;

public abstract class CommandException(string rawCommand, string message) : Exception(message)
{
	public string RawCommand { get; set; } = rawCommand;
}