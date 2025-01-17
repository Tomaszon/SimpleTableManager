namespace SimpleTableManager.Models;

[ParseFormat("Type specific format",
	"^((?<n>.+){0})?(?<v>.+)$",
	[Shared.NAMED_ARG_SEPARATOR])]
public class ConstFunctionArgument<T>(T value, ArgumentName? name = null) :
	ParsableBase<ConstFunctionArgument<T>>,
	IParsable<ConstFunctionArgument<T>>,
	IParseCore<ConstFunctionArgument<T>>,
	IConstFunctionArgument
	where T : IParsable<T>
{
	public ArgumentName? Name { get; } = name;

	public T? Value { get; set; } = value;

	object? IConstFunctionArgument.Value
	{
		get => Value!;
		set => Value = (T?)value;
	}

	IEnumerable<object> IFunctionArgument.Resolve()
	{
		return Value.Wrap().Cast<object>();
	}

	public static ConstFunctionArgument<T> ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var narg = args["n"];
		var varg = args["v"];

		var vs = varg.Value;

		var value = (T?)ContentParser.ParseConstStringValue(typeof(T), vs)!;

		ArgumentName? n = narg.Success ? Enum.Parse<ArgumentName>(narg.Value, true) : null;

		return new ConstFunctionArgument<T>(value, n);
	}

	public override string ToString()
	{
		return Value?.ToString() ?? $"({typeof(T).GetFriendlyName()})null";
	}
}