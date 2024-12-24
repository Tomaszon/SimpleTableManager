namespace SimpleTableManager.Models.Exceptions;

public class ArgumentCountException(string rawCommand, CommandReference? commandReference) : CommandException(rawCommand, "Count of arguments does not match the required amount!")
{
	public CommandReference? CommandReference { get; set; } = commandReference;
}