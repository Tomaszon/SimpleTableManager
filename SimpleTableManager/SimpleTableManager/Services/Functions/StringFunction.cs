namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, " ")]
[NamedArgument<char>(ArgumentName.Trim, ' ')]
[NamedArgument<string>(ArgumentName.Pattern, ".*")]
[FunctionMappingType(typeof(string))]
public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
{
    public override string GetFriendlyName()
    {
        return typeof(string).GetFriendlyName();
    }

    public override IEnumerable<object> ExecuteCore()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator)!;
		var trim = GetNamedArgument<char>(ArgumentName.Trim);
		var pattern = new Regex(GetNamedArgument<string>(ArgumentName.Pattern));

		return Operator switch
		{
			StringFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<object>(),

			StringFunctionOperator.Concat => string.Concat(UnwrappedUnnamedArguments).Wrap(),

			StringFunctionOperator.Join => string.Join(separator, UnwrappedUnnamedArguments).Wrap(),

			StringFunctionOperator.Len => string.Concat(UnwrappedUnnamedArguments).Length.Wrap<object>(),

			StringFunctionOperator.Split => UnwrappedUnnamedArguments.SelectMany(p => p.Split(separator)),

			StringFunctionOperator.Trim => UnwrappedUnnamedArguments.Select(p => p.Trim(trim)),

			StringFunctionOperator.Blow => UnwrappedUnnamedArguments.SelectMany(p => p.ToArray()).Cast<object>(),

			StringFunctionOperator.Like => UnwrappedUnnamedArguments.Any(pattern.IsMatch).Wrap<object>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			StringFunctionOperator.Len => typeof(int),
			StringFunctionOperator.Blow => typeof(char),

			_ => typeof(string)
		};
	}
}