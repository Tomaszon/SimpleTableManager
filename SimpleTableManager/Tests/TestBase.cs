using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class TestBase
{
	protected static IFunction CreateFunction<T>(Enum functionOperator, params IEnumerable<T> args)
		where T : IType
	{
		return FunctionCollection.GetFunction<T>(functionOperator,
		  args.Select(e => new ConstFunctionArgument<T>(e)));
	}

	protected static IFunction CreateFunction<T>(Enum functionOperator, IEnumerable<IFunctionArgument> namedArguments, params IEnumerable<T> args)
		where T : IType
	{
		return FunctionCollection.GetFunction<T>(functionOperator,
		  args.Select(e => (IFunctionArgument)new ConstFunctionArgument<T>(e)).Union(namedArguments));
	}

	protected static void CheckResults<T>(IEnumerable<object> results, IEnumerable<T> expectedValues)
	{
		for (int i = 0; i < results.Count(); i++)
		{
			CheckResult(results.ElementAt(i), expectedValues.ElementAt(i));
		}
	}

	protected static void CheckResult<T>(object result, T expectedValue)
	{
		Assert.That(result, Is.EqualTo(expectedValue));
	}
}