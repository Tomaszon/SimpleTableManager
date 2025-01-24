namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		ArgumentName? Name { get; set; }

		[MemberNotNullWhen(true, nameof(Name))]
		bool IsNamed => Name is not null;

		IEnumerable<IConvertible>? Resolve();
	}
}