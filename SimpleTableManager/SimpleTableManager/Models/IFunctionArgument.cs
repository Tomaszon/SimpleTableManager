namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		IEnumerable<object>? Resolve();
	}
}