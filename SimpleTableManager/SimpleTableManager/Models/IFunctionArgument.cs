namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		//TODO handle null values
		IEnumerable<object>? Resolve();

		bool TryResolve(out IEnumerable<object>? result)
		{
			try
			{
				result = Resolve();

				return true;
			}
			catch
			{
				result = null;

				return false;
			}
		}
	}
}