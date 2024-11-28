namespace SimpleTableManager.Models
{
	public class ReferenceFunctionArgument : IFunctionArgument
	{
		public CellReference Reference { get; set; }

		public ReferenceFunctionArgument(CellReference reference)
		{
			Reference = reference;
		}

		public IEnumerable<object>? Resolve()
		{
			return Reference.Table[Reference.Position].ContentFunction?.Execute();
		}
	}
}