using SimpleTableManager.Models;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests
{
	public class FunctionTests
	{
		[SetUp]
		public void Setup()
		{
			BorderCharacters.FromJson(@".\Configs\borderCharacters.json");
			CommandTree.FromJsonFolder(@".\Configs\Commands");
			Settings.FromJson(@".\Configs\settings.json");
			CellBorders.FromJson(@".\Configs\cellBorders.json");

			var document = new Document();
			var app = new App();

			InstanceMap.Instance.Add(() => app);
			InstanceMap.Instance.Add(() => document);
			InstanceMap.Instance.Add(() => document.GetActiveTable());
			InstanceMap.Instance.Add(() => document.GetActiveTable().GetSelectedCells());
		}

		private IFunction CreateFunction<T>(Enum functionOperator, params T[] args)
		{
			return FunctionCollection.GetFunction(typeof(T).Name, functionOperator.ToString(), null, args.Cast<object>());
		}

		public void CheckResults(IEnumerable<object> result, params object[] expectedValues)
		{
			for (int i = 0; i < result.Count(); i++)
			{
				Assert.AreEqual(result.ElementAt(i), expectedValues[i]);
			}
		}

		[Test]
		public void Bool1()
		{
			var fn = CreateFunction<bool>(BooleanFunctionOperator.Const, true, false);

			var result = fn.Execute();

			CheckResults(result, true, false);
		}
	}
}