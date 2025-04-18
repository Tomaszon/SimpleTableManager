namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		ArgumentName? Name { get; }

		[MemberNotNullWhen(true, nameof(Name))]
		bool IsNamed => Name is not null;

		public object? GroupingId { get; set; }

		IEnumerable<object> Resolve(bool ignoreNullReference);
	}
}