namespace SimpleTableManager.Models
{
	public class PartialKeyException : CommandException
	{
		public string PartialKey { get; set; }

		public PartialKeyException(string rawCommand, string partialKey) : base(rawCommand, $"Partial key '{partialKey}' in command")
		{
			PartialKey = partialKey;
		}
	}
}
