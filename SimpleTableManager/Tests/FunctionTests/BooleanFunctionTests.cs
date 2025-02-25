namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class BooleanFunctionTests : TestBase
{
	[TestCase(BooleanFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(BooleanFunctionOperator.Or, new[] { false, true }, true)]
	[TestCase(BooleanFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	public void BooleanTest(BooleanFunctionOperator operation, bool[] values, params bool[] results)
	{
		var fn = CreateFunction(operation, values.Select(e => (BooleanType)e));

		CheckResults(fn.Execute(), results.Select(e => (BooleanType)e));
	}
}