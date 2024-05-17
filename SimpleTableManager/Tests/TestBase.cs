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

	public static IFunction CreateFunction<T>(Enum functionOperator, T[] args)
	{
		return CreateFunction(functionOperator, null, args);
	}

	public static IFunction CreateFunction<T>(Enum functionOperator, Dictionary<ArgumentName, string>? namedArguments, params T[] args)
	{
		return FunctionCollection.GetFunction(typeof(T).GetFriendlyName(), functionOperator.ToString(), namedArguments, args.Cast<object>());
	}

	public static void CheckResults<T>(IEnumerable<object> result, IEnumerable<T> expectedValues)
	{
		for (int i = 0; i < result.Count(); i++)
		{
			Assert.That(result.ElementAt(i), Is.EqualTo(expectedValues.ElementAt(i)));
		}
	}
}