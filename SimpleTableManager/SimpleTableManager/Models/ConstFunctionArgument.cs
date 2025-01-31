namespace SimpleTableManager.Models;

[ParseFormat("Type specific format",
	"^((?<n>.+)Arg0)?(?<v>.+)$",
	[Shared.NAMED_ARG_SEPARATOR])]
[method: JsonConstructor]
public class ConstFunctionArgument<T>(T? value) :
	ParsableBase<ConstFunctionArgument<T>>,
	IParsable<ConstFunctionArgument<T>>,
	IParsableCore<ConstFunctionArgument<T>>,
	IConstFunctionArgument
	where T : IParsable<T>, IConvertible
{
	public ArgumentName? Name { get; set; }

	public T? Value { get; set; } = value;

	public IConvertible? NamedValue { get; set; }

	IConvertible? IConstFunctionArgument.Value
	{
		get => Value!;
		set => Value = (T?)value;
	}

	public ConstFunctionArgument(ArgumentName argumentName, IConvertible? namedValue) : this(default)
	{
		Name = argumentName;
		NamedValue = namedValue;
	}

	public IEnumerable<IConvertible> Resolve()
	{
		return (NamedValue ?? Value).Wrap()!;
	}

	public static ConstFunctionArgument<T> ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var narg = args["n"];
		var varg = args["v"];

		if (narg.Success)
		{
			var name = Enum.TryParse<ArgumentName>(narg.Value, true, out var n) ?
				n : throw new FormatException($"Argument name '{narg.Value}' not found. Possible values: {string.Join("' '", Enum.GetValues<ArgumentName>())}");

			return new ConstFunctionArgument<T>(name, varg.Value);
		}
		else
		{
			var value = (T?)ContentParser.ParseConstStringValue(typeof(T), varg.Value)!;

			return new ConstFunctionArgument<T>(value);
		}
	}

	public override string ToString()
	{
		return (NamedValue is not null ? $"{Name}:{NamedValue}" : null) ??
			Value?.ToString() ??
			$"({typeof(T).GetFriendlyName()})null";
	}
}