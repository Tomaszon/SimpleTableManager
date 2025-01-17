namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		public ArgumentName? Name { get; }

		public bool IsNamed => Name is not null;

		IEnumerable<object>? Resolve();
	}
}