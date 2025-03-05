using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class BooleanFunctionTests : TestBase
{
	[TestCase(BooleanFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(BooleanFunctionOperator.Or, new[] { false, true }, true)]
	[TestCase(BooleanFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	[TestCase(BooleanFunctionOperator.Greater, new[] { true, false }, true)]
	[TestCase(BooleanFunctionOperator.GreaterOrEquals, new[] { true, true }, true)]
	[TestCase(BooleanFunctionOperator.Less, new[] { false, true }, true)]
	[TestCase(BooleanFunctionOperator.LessOrEquals, new[] { false, false }, true)]
	[TestCase(BooleanFunctionOperator.Equals, new[] { false, false }, true)]
	[TestCase(BooleanFunctionOperator.NotEquals, new[] { true, false }, true)]
	public void BooleanTest(BooleanFunctionOperator operation, bool[] values, params bool[] results)
	{
		var fn = CreateFunction(operation, values.Select(e => (FormattableBoolean)e));

		CheckResults(fn.Execute(), results.Select(e => (FormattableBoolean)e));
	}

	[Test]
	public void BooleanTest2()
	{
		var fn = FunctionCollection.GetFunction<FormattableBoolean>(BooleanFunctionOperator.Const, [new ReferenceFunctionArgument(new(Guid.Empty, new(1, 1, false, false)))]);

		fn.ShiftReferenceArgumentPositions(new(1, 2));

		CheckResult(((ReferenceFunctionArgument)fn.Arguments[0]).Reference.ReferencedPositions[0], new Position(2, 3));
	}
}