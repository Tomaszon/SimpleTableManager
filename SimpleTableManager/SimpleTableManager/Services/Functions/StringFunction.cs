namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, " ")]
[NamedArgument<char>(ArgumentName.Trim, ' ')]
[NamedArgument<string>(ArgumentName.Pattern, ".*")]
[FunctionMappingType(typeof(string))]
public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator)!;
		var trim = GetNamedArgument<char>(ArgumentName.Trim);
		var pattern = new Regex(GetNamedArgument<string>(ArgumentName.Pattern));

		return Operator switch
		{
			StringFunctionOperator.Const => ConvertedUnwrappedArguments.Cast<object>(),

			StringFunctionOperator.Con => string.Concat(ConvertedUnwrappedArguments).Wrap(),

			StringFunctionOperator.Join => string.Join(separator, ConvertedUnwrappedArguments).Wrap(),

			StringFunctionOperator.Len => string.Concat(ConvertedUnwrappedArguments).Length.Wrap<object>(),

			StringFunctionOperator.Split => ConvertedUnwrappedArguments.SelectMany(p => p.Split(separator)),

			StringFunctionOperator.Trim => ConvertedUnwrappedArguments.Select(p => p.Trim(trim)),

			StringFunctionOperator.Blow => ConvertedUnwrappedArguments.SelectMany(p => p.ToArray()).Cast<object>(),

			StringFunctionOperator.Like => ConvertedUnwrappedArguments.Any(pattern.IsMatch).Wrap<object>(),

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