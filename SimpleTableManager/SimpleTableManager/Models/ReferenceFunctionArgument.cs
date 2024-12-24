namespace SimpleTableManager.Models
{
	public class ReferenceFunctionArgument(CellReference reference) : IFunctionArgument
	{
		public CellReference Reference { get; set; } = reference;

		public IEnumerable<object>? Resolve()
		{
			return Reference.Table[Reference.Position].ContentFunction?.Execute();
		}
	}
}