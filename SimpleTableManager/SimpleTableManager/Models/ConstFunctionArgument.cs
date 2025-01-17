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

		ArgumentName? name = null;

		if (narg.Success)
		{
			name = Enum.TryParse<ArgumentName>(narg.Value, true, out var n) ?
				(ArgumentName?)n : throw new FormatException($"Argument name '{narg.Value}' not found. Possible values: {string.Join("' '", Enum.GetValues<ArgumentName>())}");
		}

		return new ConstFunctionArgument<T>(value, name);
	}

	public override string ToString()
	{
		return Value?.ToString() ?? $"({typeof(T).GetFriendlyName()})null";
	}
}