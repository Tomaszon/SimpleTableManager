using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class BooleanFunctionTests : TestBase
{
	[TestCase(LogicalFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(LogicalFunctionOperator.Or, new[] { false, true }, true)]
	[TestCase(LogicalFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	[TestCase(LogicalFunctionOperator.Min, new[] { false, true }, false)]
	[TestCase(LogicalFunctionOperator.Max, new[] { false, true }, true)]
	[TestCase(LogicalFunctionOperator.Greater, new[] { true }, true)]
	[TestCase(LogicalFunctionOperator.GreaterOrEquals, new[] { true }, true)]
	[TestCase(LogicalFunctionOperator.Less, new[] { false }, true)]
	[TestCase(LogicalFunctionOperator.LessOrEquals, new[] { false }, true)]
	[TestCase(LogicalFunctionOperator.Equals, new[] { false }, true)]
	[TestCase(LogicalFunctionOperator.NotEquals, new[] { true }, true)]
	[TestCase(LogicalFunctionOperator.Greater, new[] { true, false }, true)]
	[TestCase(LogicalFunctionOperator.GreaterOrEquals, new[] { true, true }, true)]
	[TestCase(LogicalFunctionOperator.Less, new[] { false, true }, true)]
	[TestCase(LogicalFunctionOperator.LessOrEquals, new[] { false, false }, true)]
	[TestCase(LogicalFunctionOperator.Equals, new[] { false, false }, true)]
	[TestCase(LogicalFunctionOperator.NotEquals, new[] { true, false }, true)]
	[TestCase(LogicalFunctionOperator.IsNull, new[] { true }, false)]
	[TestCase(LogicalFunctionOperator.IsNotNull, new[] { true }, true)]
	public void BooleanTest(LogicalFunctionOperator operation, bool[] values, params bool[] results)
	{
		var fn = CreateFunction(operation, values.Select(e => (FormattableBoolean)e));

		CheckResults(fn.Execute(), results.Select(e => (FormattableBoolean)e));
	}

	[Test]
	public void BooleanTest2()
	{
		var fn = FunctionCollection.GetFunction<FormattableBoolean>(LogicalFunctionOperator.Const, [new ReferenceFunctionArgument(new(Guid.Empty, new(1, 1, false, false)))]);

		fn.ShiftReferenceArgumentPositions(Guid.Empty, new(1, 2));

		CheckResult(((ReferenceFunctionArgument)fn.Arguments[0]).Reference.ReferencedPositions[0], new Position(2, 3));
	}

	[Test]
	public void BooleanTest3()
	{
		var fn = CreateFunction(LogicalFunctionOperator.Const, (FormattableBoolean)true);

		CheckResult(fn.GetError(), "None");

		fn.SetError("ASD");

		var e = ((FunctionBase<LogicalFunctionOperator, FormattableBoolean, FormattableBoolean>)fn).GetInvalidOperatorException();

		CheckResult(fn.GetError(), "ASD");
		CheckResult(fn.GetOutType(), typeof(FormattableBoolean));
		CheckResult(fn.GetInType(), typeof(FormattableBoolean));
		CheckResult(fn.GetFriendlyName(), typeof(FormattableBoolean).GetFriendlyName());
		CheckResult(fn.Operator, LogicalFunctionOperator.Const);
		CheckResult(e.GetType(), typeof(InvalidOperationException));
	}
}