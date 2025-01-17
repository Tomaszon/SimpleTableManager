namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		public ArgumentName? Name { get; set; }

		[MemberNotNullWhen(true, nameof(Name))]
		public bool IsNamed => Name is not null;

		IEnumerable<object>? Resolve();
	}
}