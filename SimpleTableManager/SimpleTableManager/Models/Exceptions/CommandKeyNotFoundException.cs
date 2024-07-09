namespace SimpleTableManager.Models.Exceptions;

public class CommandKeyNotFoundException : CommandException
{
	public CommandKeyNotFoundException(string rawCommand, string key) : base(rawCommand, $"Unknown command key '{key}' in '{rawCommand}'") { }
}