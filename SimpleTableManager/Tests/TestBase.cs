using SimpleTableManager.Models;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests;

public class TestBase
{
	[OneTimeSetUp]
	public void Setup()
	{
		Settings.FromJson(@".\Configs\settings.json");
	}

	protected static IFunction CreateFunction<T>(Enum functionOperator, T[] args)
	where T: IParsable<T>
	{
		return CreateFunction(functionOperator, null, args);
	}

	protected static IFunction CreateFunction<T>(Enum functionOperator, Dictionary<ArgumentName, string>? namedArguments, params T[] args)
	where T : IParsable<T>
	{
		return FunctionCollection.GetFunction(typeof(T).GetFriendlyName(), functionOperator.ToString(), namedArguments, args.Select(e => new ConstFunctionArgument<T>(e)));
	}

	protected static void CheckResults<T>(IEnumerable<object> result, IEnumerable<T> expectedValues)
	{
		for (int i = 0; i < result.Count(); i++)
		{
			Assert.That(result.ElementAt(i), Is.EqualTo(expectedValues.ElementAt(i)));
		}
	}
}