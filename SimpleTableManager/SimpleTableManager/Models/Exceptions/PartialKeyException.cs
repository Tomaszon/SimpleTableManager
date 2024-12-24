namespace SimpleTableManager.Models.Exceptions;

public class PartialKeyException(string rawCommand, string? partialKey) : CommandException(rawCommand, $"Partial key '{partialKey}' in command")
{
	public string? PartialKey { get; set; } = partialKey;
}