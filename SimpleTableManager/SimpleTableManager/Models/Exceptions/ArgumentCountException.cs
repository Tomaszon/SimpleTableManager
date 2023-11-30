namespace SimpleTableManager.Models.Exceptions;

public class ArgumentCountException : CommandException
{
	public CommandReference? CommandReference { get; set; }

	public ArgumentCountException(string rawCommand, CommandReference? commandReference) : base(rawCommand, "Count of arguments does not match the required amount!")
	{
		CommandReference = commandReference;
	}
}