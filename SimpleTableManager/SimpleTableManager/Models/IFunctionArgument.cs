namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		//TODO handle null values
		IEnumerable<object>? Resolve();

		bool TryResolve(out IEnumerable<object>? result, [NotNullWhen(false)] out string? error)
		{
			try
			{
				result = Resolve();

				error = null;

				return true;
			}
			catch (Exception ex)
			{
				result = null;

				error = ex.Message;

				return false;
			}
		}
	}
}