namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		//TODO handle null values
		IEnumerable<object>? Resolve();
	}
}