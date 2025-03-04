namespace SimpleTableManager.Models;

[ParseFormat("Type specific format",
	"^(?<n>.+)Arg0(?<v>.+)$",
	[Shared.NAMED_ARG_SEPARATOR])]
[method: JsonConstructor]
public class NamedConstFunctionArgument(ArgumentName name, object value, object? groupingId = null) :
	ParsableBase<NamedConstFunctionArgument>,
	IParsable<NamedConstFunctionArgument>,
	IParsableCore<NamedConstFunctionArgument>,
	IConstFunctionArgument
{
	public ArgumentName? Name { get; set; } = name;

	public object Value { get; set; } = value;

	public object? GroupingId { get; set; } = groupingId;

	public IEnumerable<object> Resolve(bool ignoreNullReference)
	{
		return Value.Wrap()!;
	}

	public static NamedConstFunctionArgument ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var narg = args["n"];
		var varg = args["v"];

		var name = Enum.TryParse<ArgumentName>(narg.Value, true, out var n) ?
		n : throw new FormatException($"Argument name '{narg.Value}' not found. Possible values: {string.Join("' '", Enum.GetValues<ArgumentName>())}");

		return new NamedConstFunctionArgument(name, varg.Value);
	}

	public override string ToString()
	{
		return $"{Name}:{Value ?? "null"}";
	}
}
