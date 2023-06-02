namespace SimpleTableManager.Services.Functions
{
	public interface IFunction
	{
		Dictionary<ArgumentName, string> NamedArguments { get; }

		IEnumerable<object> Arguments { get; }

		Enum Operator { get; }

		IEnumerable<object> Execute();

		Type GetReturnType();

		string GetError();
	}
}