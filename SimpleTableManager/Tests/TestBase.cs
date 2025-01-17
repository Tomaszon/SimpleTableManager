using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests;

public class TestBase
{
	protected static IFunction CreateFunction<T>(Enum functionOperator, params IEnumerable<T> args)
	where T : IParsable<T>
	{
		return CreateFunction(functionOperator, null, args);
	}

	protected static IFunction CreateFunction<T>(Enum functionOperator, Dictionary<ArgumentName, string>? namedArguments, params IEnumerable<T> args)
	where T : IParsable<T>
	{
		// return FunctionCollection.GetFunction<T>(functionOperator.ToString(),
		//  namedArguments?.ToDictionary(k => k.Key, v => (IFunctionArgument)new ConstFunctionArgument<string>(v.Value)),
		//   args.Select(e => new ConstFunctionArgument<T>(e)));
		return null;
	}

	protected static void CheckResults<T>(IEnumerable<object> result, IEnumerable<T> expectedValues)
	{
		for (int i = 0; i < result.Count(); i++)
		{
			Assert.That(result.ElementAt(i), Is.EqualTo(expectedValues.ElementAt(i)));
		}
	}
}