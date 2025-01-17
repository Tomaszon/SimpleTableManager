namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, "")]
[NamedArgument<int>(ArgumentName.Count, 1)]
[FunctionMappingType(typeof(char))]
public class CharacterFunction : FunctionBase<CharacterFunctionOperator, char, object>
{
	public override string GetFriendlyName()
	{
		return typeof(char).GetFriendlyName();
	}

	public override IEnumerable<object> ExecuteCore()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);
		var count = GetNamedArgument<int>(ArgumentName.Count);

		return Operator switch
		{
			CharacterFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<object>(),

			CharacterFunctionOperator.Concat => string.Concat(UnwrappedUnnamedArguments).Wrap(),

			CharacterFunctionOperator.Join => string.Join(separator, UnwrappedUnnamedArguments).Wrap(),

			CharacterFunctionOperator.Repeat => UnwrappedUnnamedArguments.Select(c => new string(c, count)),

			_ => throw GetInvalidOperatorException()
		};
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