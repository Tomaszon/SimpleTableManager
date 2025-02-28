namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, " ")]
[NamedArgument<char>(ArgumentName.Trim, ' ')]
[NamedArgument<string>(ArgumentName.Pattern, ".*")]
[FunctionMappingType(typeof(string))]
public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			StringFunctionOperator.Const => UnwrappedUnnamedArguments,

			StringFunctionOperator.Concat => string.Concat(UnwrappedUnnamedArguments).Wrap(),

			StringFunctionOperator.Join => Join().Wrap(),

			StringFunctionOperator.Len => string.Concat(UnwrappedUnnamedArguments).Length.Wrap<IConvertible>(),

			StringFunctionOperator.Split => Split(),

			StringFunctionOperator.Trim => Trim(),

			StringFunctionOperator.Blow => UnwrappedUnnamedArguments.SelectMany(p => p.ToArray()).Cast<IConvertible>(),

			StringFunctionOperator.Like => Like().Wrap<object>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private string Join()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);

		return string.Join(separator, UnwrappedUnnamedArguments);
	}

	private IEnumerable<string> Split()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);

		return UnwrappedUnnamedArguments.SelectMany(p => p.Split(separator));
	}

	private IEnumerable<string> Trim()
	{
		var trim = GetNamedArgument<char>(ArgumentName.Trim);

		return UnwrappedUnnamedArguments.Select(p => p.Trim(trim));
	}

	private bool Like()
	{
		var pattern = new Regex(GetNamedArgument<string>(ArgumentName.Pattern));

		return UnwrappedUnnamedArguments.Any(pattern.IsMatch);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			StringFunctionOperator.Len => typeof(int),
			StringFunctionOperator.Blow => typeof(char),
			StringFunctionOperator.Like => typeof(bool),

			_ => typeof(string)
		};
	}
}