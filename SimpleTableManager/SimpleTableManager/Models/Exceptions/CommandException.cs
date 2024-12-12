namespace SimpleTableManager.Models.Exceptions;

public abstract class CommandException : Exception
{
	public string RawCommand { get; set; }

	public CommandException(string rawCommand, string message) : base(message)
	{
		RawCommand = rawCommand;
	}
}