namespace SimpleTableManager.Models
{
	public interface IConstFunctionArgument : IFunctionArgument
	{
		object? Value { get; set; }
	}
}