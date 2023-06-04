using SimpleTableManager.Models;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests
{
	public class FunctionTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			Settings.FromJson(@".\Configs\settings.json");
		}

		private IFunction CreateFunction<T>(Enum functionOperator, params T[] args)
		{
			return FunctionCollection.GetFunction(typeof(T).Name, functionOperator.ToString(), null, args.Cast<object>());
		}

		public void CheckResults<T>(IEnumerable<object> result, IEnumerable<T> expectedValues)
		{
			for (int i = 0; i < result.Count(); i++)
			{
				Assert.AreEqual(expectedValues.ElementAt(i), result.ElementAt(i));
			}
		}

		[Test]
		public void Bool1()
		{
			var values = new[] { true, false };

			var fn = CreateFunction<bool>(BooleanFunctionOperator.Const, values);

			CheckResults(fn.Execute(), values);
		}

		[Test]
		public void DateTime1()
		{
			var values = new[] { new DateTime(1, 2, 3, 4, 5, 59), new DateTime(6, 5, 4, 3, 2, 11) };

			var fn = CreateFunction<DateTime>(DateTimeFunctionOperator.Const, values);

			CheckResults(fn.Execute(), values);

			fn = CreateFunction<DateTime>(DateTimeFunctionOperator.Sum, values);

			CheckResults(fn.Execute(), new DateTime(7, 7, 7, 7, 8, 10).Wrap());
		}
	}
}