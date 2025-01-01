namespace SimpleTableManager.Models
{
	public class ReferenceFunctionArgument(CellReference reference) : IFunctionArgument
	{
		public CellReference Reference { get; set; } = reference;

		public IEnumerable<object>? Resolve()
		{
			var doc = InstanceMap.Instance.GetInstance<Document>()!;

			var table = doc[Reference.ReferencedTableId];

			return table[Reference.ReferencedPosition].ContentFunction?.Execute();
		}

		public bool TryResolve(out IEnumerable<object>? result, [NotNullWhen(false)] out string? error)
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