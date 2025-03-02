using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Separator, "")]
[NamedArgument<int>(ArgumentName.Count, 1)]
[FunctionMappingType(typeof(char))]
public class CharacterFunction : FunctionBase<CharacterFunctionOperator, char, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			CharacterFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<object>(),
			CharacterFunctionOperator.Concat => string.Concat(UnwrappedUnnamedArguments).Wrap(),
			CharacterFunctionOperator.Join => Join().Wrap(),
			CharacterFunctionOperator.Repeat => Repeat(),
			CharacterFunctionOperator.Greater => Greater().Wrap(),
			CharacterFunctionOperator.Less => Less().Wrap(),
			CharacterFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap(),
			CharacterFunctionOperator.LessOrEquals => LessOrEquals().Wrap(),
			CharacterFunctionOperator.Equals => Equals().Wrap(),
			CharacterFunctionOperator.NotEquals => NotEquals().Wrap(),

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
			>= CharacterFunctionOperator.Greater and
			<= CharacterFunctionOperator.NotEquals => typeof(FormattableBoolean),

			_ => typeof(string)
		};
	}
}