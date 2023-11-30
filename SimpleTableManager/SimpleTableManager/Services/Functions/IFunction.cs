namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	Dictionary<ArgumentName, string> NamedArguments { get; }

	IEnumerable<object> Arguments { get; }

	Enum Operator { get; }

	IEnumerable<object> Execute();

	IEnumerable<string> ExecuteAndFormat();

	Type? GetReturnType();

	TParse? GetNamedArgument<TParse>(ArgumentName key) where TParse : IParsable<TParse>;

	string GetError();

	Type GetInType();
}