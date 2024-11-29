
using System.Text.RegularExpressions;

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
			StringFunctionOperator.Const => UnwrappedArguments.Cast<object>(),

			StringFunctionOperator.Con => Concat().Wrap(),

			StringFunctionOperator.Join => Join(separator).Wrap(),

			StringFunctionOperator.Len => Concat().Length.Wrap<object>(),

			StringFunctionOperator.Split => UnwrappedArguments.SelectMany(p => p.Split(separator)),

			StringFunctionOperator.Trim => UnwrappedArguments.Select(p => p.Trim(trim)),

			StringFunctionOperator.Blow => UnwrappedArguments.SelectMany(p => p.ToArray()).Cast<object>(),

			StringFunctionOperator.Like => UnwrappedArguments.Any(pattern.IsMatch).Wrap<object>(),

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

	private string Concat()
	{
		// return string.Concat(string.Concat(Arguments), string.Concat(ReferenceArguments));
		return string.Concat(UnwrappedArguments);
	}

	private string Join(string separator)
	{
		//var value = string.Join(separator, string.Join(separator, Arguments), string.Join(separator, ReferenceArguments));
		return string.Join(separator, UnwrappedArguments);
	}
}