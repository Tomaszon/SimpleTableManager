namespace SimpleTableManager.Models
{
	public interface IConstFunctionArgument : IFunctionArgument
	{
		IConvertible? Value { get; set; }

		IConvertible? RawValue { get; set; }
	}
}