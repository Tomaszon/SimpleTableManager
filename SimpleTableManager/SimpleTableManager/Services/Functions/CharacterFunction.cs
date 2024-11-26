
namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, "")]
[NamedArgument<int>(ArgumentName.Count, 1)]
[FunctionMappingType(typeof(char))]
public class CharacterFunction : FunctionBase<CharacterFunctionOperator, char, object>
{
	public override IEnumerable<object> Execute()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);
		var count = GetNamedArgument<int>(ArgumentName.Count);

		return Operator switch
		{
			CharacterFunctionOperator.Const => UnwrappedArguments.Cast<object>(),

			CharacterFunctionOperator.Concat => string.Concat(UnwrappedArguments).Wrap(),

			CharacterFunctionOperator.Join => string.Join(separator, UnwrappedArguments).Wrap(),

			CharacterFunctionOperator.Repeat => UnwrappedArguments.Select(c => new string(c, count)),

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