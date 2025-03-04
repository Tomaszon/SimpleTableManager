namespace SimpleTableManager.Models;

[ParseFormat("Type specific format",
	"^(?<v>.+)$")]
[method: JsonConstructor]
public class ConstFunctionArgument<T>(T value, object? groupingId = null) :
	ParsableBase<ConstFunctionArgument<T>>,
	IParsable<ConstFunctionArgument<T>>,
	IParsableCore<ConstFunctionArgument<T>>,
	IConstFunctionArgument
	where T : IParsable<T>, IConvertible, IComparable
{
	public ArgumentName? Name => null;

	public T Value { get; set; } = value;

	public object? GroupingId { get; set; } = groupingId;

	object IConstFunctionArgument.Value
	{
		get => Value!;
		set => Value = (T)value;
	}

	public IEnumerable<object> Resolve(bool ignoreNullReference)
	{
		return Value.Wrap().Cast<object>();
	}

	public static ConstFunctionArgument<T> ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var varg = args["v"];

		var value = (T?)ContentParser.ParseConstStringValue(typeof(T), varg.Value)!;

		return new ConstFunctionArgument<T>(value);
	}

	public override string ToString()
	{
		return Value?.ToString() ??
			$"({typeof(T).GetFriendlyName()})null";
	}
}