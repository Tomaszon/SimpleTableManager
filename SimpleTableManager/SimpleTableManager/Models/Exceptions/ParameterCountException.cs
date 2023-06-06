namespace SimpleTableManager.Models.Exceptions
{
	public class ParameterCountException : CommandException
	{
		public CommandReference? CommandReference { get; set; }

		public ParameterCountException(string rawCommand, CommandReference? commandReference) : base(rawCommand, "Count of parameters does not match the required amount")
		{
			CommandReference = commandReference;
		}
	}
}
