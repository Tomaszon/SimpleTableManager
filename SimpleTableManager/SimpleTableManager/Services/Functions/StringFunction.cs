namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, " ")]
[NamedArgument<char>(ArgumentName.Trim, ' ')]
[NamedArgument<string>(ArgumentName.Pattern, ".*")]
[FunctionMappingType(typeof(StringType))]
public class StringFunction : FunctionBase<StringFunctionOperator, StringType, IType>
{
	public override IEnumerable<IType> ExecuteCore()
	{
		return Operator switch
		{
			StringFunctionOperator.Const => UnwrappedUnnamedArguments,

			StringFunctionOperator.Concat => StringType.Concat(UnwrappedUnnamedArguments).Wrap(),

			StringFunctionOperator.Join => Join().Wrap(),

			StringFunctionOperator.Len => StringType.Concat(UnwrappedUnnamedArguments).Length.Wrap(),

			StringFunctionOperator.Split => Split(),

			StringFunctionOperator.Trim => Trim(),

			StringFunctionOperator.Blow => UnwrappedUnnamedArguments.SelectMany(p => p.ToArray()),

			StringFunctionOperator.Like => Like().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private StringType Join()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);

		return string.Join(separator, UnwrappedUnnamedArguments);
	}

	private IEnumerable<StringType> Split()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);

		return UnwrappedUnnamedArguments.SelectMany(p => p.Split(separator));
	}

	private IEnumerable<StringType> Trim()
	{
		var trim = GetNamedArgument<char>(ArgumentName.Trim);

		return UnwrappedUnnamedArguments.Select(p => p.Trim(trim));
	}

	private BooleanType Like()
	{
		var pattern = new Regex(GetNamedArgument<string>(ArgumentName.Pattern));

		return UnwrappedUnnamedArguments.Any(a => pattern.IsMatch(a));
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			StringFunctionOperator.Len => typeof(IntegerType),
			StringFunctionOperator.Blow => typeof(CharacterType),
			StringFunctionOperator.Like => typeof(BooleanType),

			_ => typeof(StringType)
		};
	}
}