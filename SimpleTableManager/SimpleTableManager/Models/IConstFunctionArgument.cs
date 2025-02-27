namespace SimpleTableManager.Models
{
	public interface IConstFunctionArgument : IFunctionArgument
	{
		IType? Value { get; set; }

		IType? NamedValue { get; set; }
	}
}