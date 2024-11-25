using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	Dictionary<ArgumentName, string> NamedArguments { get; set; }

	IEnumerable<IFunctionArgument> Arguments { get; set; }

	Enum Operator { get; set; }

	IEnumerable<object> Execute();

	IEnumerable<string> ExecuteAndFormat();

	Type GetOutType();

	TParse? GetNamedArgument<TParse>(ArgumentName key) where TParse : IParsable<TParse>;

	string GetError();

	Type GetInType();
}