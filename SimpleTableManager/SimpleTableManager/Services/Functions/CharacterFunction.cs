
namespace SimpleTableManager.Services.Functions;

[NamedArgument(ArgumentName.Separator, "")]
[NamedArgument(ArgumentName.Count, 1)]
public class CharacterFunction : FunctionBase<CharacterFunctionOperator, char, object>
{
	public override IEnumerable<object> Execute()
	{
		var separator = GetNamedArgument<string>(ArgumentName.Separator);
		var count = GetNamedArgument<int>(ArgumentName.Count);

		return Operator switch
		{
			CharacterFunctionOperator.Const => Arguments.Cast<object>(),

			CharacterFunctionOperator.Concat => string.Concat(Arguments).Wrap(),

			CharacterFunctionOperator.Join => string.Join(separator, Arguments).Wrap(),

			CharacterFunctionOperator.Repeat => Arguments.Select(c => new string(c, count)),

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