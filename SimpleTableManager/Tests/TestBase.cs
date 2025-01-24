using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests;

public class TestBase
{
	protected static IFunction CreateFunction<T>(Enum functionOperator, params IEnumerable<T> args)
		where T : IParsable<T>, IConvertible
	{
		return FunctionCollection.GetFunction<T>(functionOperator,
		  args.Select(e => new ConstFunctionArgument<T>(e)));
	}

	protected static IFunction CreateFunction<T>(Enum functionOperator, IEnumerable<IFunctionArgument> namedArguments, params IEnumerable<T> args)
		where T : IParsable<T>, IConvertible
	{
		return FunctionCollection.GetFunction<T>(functionOperator,
		  args.Select(e => (IFunctionArgument)new ConstFunctionArgument<T>(e)).Union(namedArguments));
	}

	protected static void CheckResults<T>(IEnumerable<object> result, IEnumerable<T> expectedValues)
	{
		for (int i = 0; i < result.Count(); i++)
		{
			Assert.That(result.ElementAt(i), Is.EqualTo(expectedValues.ElementAt(i)));
		}
	}
}