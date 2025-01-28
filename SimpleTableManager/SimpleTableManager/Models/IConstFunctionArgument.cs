namespace SimpleTableManager.Models
{
	public interface IConstFunctionArgument : IFunctionArgument
	{
		IConvertible? Value { get; set; }

		IConvertible? NamedValue { get; set; }
	}
}