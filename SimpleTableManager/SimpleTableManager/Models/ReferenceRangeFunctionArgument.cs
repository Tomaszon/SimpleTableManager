namespace SimpleTableManager.Models
{
    public class ReferenceRangeFunctionArgument(CellReferenceRange reference) : IFunctionArgument
	{
		public CellReferenceRange Reference { get; set; } = reference;

		public IEnumerable<object>? Resolve()
		{
			var doc = InstanceMap.Instance.GetInstance<Document>()!;

			var table = doc[Reference.ReferencedTableId];

			return Reference.Compile().SelectMany(r => table[r.ReferencedPosition].ContentFunction?.Execute() is var result && result is not null ? result : throw new NullReferenceException());
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