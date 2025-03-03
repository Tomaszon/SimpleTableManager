using SimpleTableManager.Models.Enumerations.FunctionOperators;

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
			StringFunctionOperator.Split => Split(),
			StringFunctionOperator.Trim => Trim(),
			StringFunctionOperator.Join => Join().Wrap(),
			StringFunctionOperator.Len => string.Concat(UnwrappedUnnamedArguments).Length.Wrap<IConvertible>(),
			StringFunctionOperator.Blow => UnwrappedUnnamedArguments.SelectMany(p => p.ToArray()).Cast<IConvertible>(),
			StringFunctionOperator.Like => Like().Wrap<object>(),
			StringFunctionOperator.Greater => Greater().Wrap(),
			StringFunctionOperator.Less => Less().Wrap(),
			StringFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap(),
			StringFunctionOperator.LessOrEquals => LessOrEquals().Wrap(),
			StringFunctionOperator.Equals => Equals().Wrap(),
			StringFunctionOperator.NotEquals => NotEquals().Wrap(),

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

	private FormattableBoolean Like()
	{
		var pattern = new Regex(GetNamedArgument<string>(ArgumentName.Pattern));

		return UnwrappedUnnamedArguments.Any(pattern.IsMatch);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			StringFunctionOperator.Len => typeof(long),
			StringFunctionOperator.Blow => typeof(char),
			>= StringFunctionOperator.Like and
			<= StringFunctionOperator.NotEquals => typeof(FormattableBoolean),

			_ => typeof(string)
		};
	}
}