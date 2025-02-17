namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, "")]
[NamedArgument<int>(ArgumentName.Count, 1)]
[FunctionMappingType(typeof(char))]
public class CharacterFunction : FunctionBase<CharacterFunctionOperator, char, IConvertible>
{
	public override IEnumerable<IConvertible> ExecuteCore()
	{
		return Operator switch
		{
			CharacterFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<IConvertible>(),

			CharacterFunctionOperator.Concat => string.Concat(UnwrappedUnnamedArguments).Wrap(),

			CharacterFunctionOperator.Join => Join().Wrap(),

			CharacterFunctionOperator.Repeat => Repeat(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private string Join()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);

		return string.Join(separator, UnwrappedUnnamedArguments);
	}

	private IEnumerable<string> Repeat()
	{
		var count = GetNamedArgument<int>(ArgumentName.Count);

		return UnwrappedUnnamedArguments.Select(c => new string(c, count));
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			CharacterFunctionOperator.Const => typeof(char),

			_ => typeof(string)
		};
	}
}