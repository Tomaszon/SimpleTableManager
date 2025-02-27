namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class BooleanFunctionTests : TestBase
{
	[TestCase(BooleanFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(BooleanFunctionOperator.Or, new[] { false, true }, true)]
	[TestCase(BooleanFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	public void BooleanTest(BooleanFunctionOperator operation, bool[] values, params bool[] results)
	{
		// var fn = CreateFunction(operation, values.CastTo<bool, BooleanType>());
		//
		// CheckResults(fn.Execute(), results.CastTo<bool, BooleanType>());
	}
}