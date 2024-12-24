namespace SimpleTableManager.Models.Exceptions;

public class CommandKeyNotFoundException(string rawCommand, string key) : CommandException(rawCommand, $"Unknown command key '{key}' in '{rawCommand}'")
{
}