using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class LogicalFunctionTests : TestBase
{
	[Test]
	public void BooleanTest()
	{
		var fb = FormattableBoolean.Parse("yes", null);

		CheckResults([fb.Equals(true), fb.Equals(null)], [true, false]);
		CheckResults<object>([fb!.ToBoolean(null), fb!.ToInt32(null), fb!.ToInt64(null), fb!.ToDouble(null)], [true, 1, 1L, 1d]);
		CheckResults([fb.ToString(), fb.ToString("yn", null), fb.ToString("yesno", null), fb.ToString("10", null)], ["True", "Y", "Yes", "1"]);
	}

	[TestCase(LogicalFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	[TestCase(LogicalFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(LogicalFunctionOperator.Or, new[] { false, true }, true)]
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
	[TestCase(LogicalFunctionOperator.IsNull, new bool[] { }, true)]
	[TestCase(LogicalFunctionOperator.IsNotNull, new bool[] { }, false)]
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

	[TestCase(LogicalFunctionOperator.Not, new[] { TrileanValue.False, TrileanValue.True, TrileanValue.Unknown }, new[] { TrileanValue.True, TrileanValue.False, TrileanValue.Unknown })]
	[TestCase(LogicalFunctionOperator.And, new[] { TrileanValue.True, TrileanValue.False }, TrileanValue.False)]
	[TestCase(LogicalFunctionOperator.And, new[] { TrileanValue.True, TrileanValue.Unknown }, TrileanValue.Unknown)]
	[TestCase(LogicalFunctionOperator.Or, new[] { TrileanValue.False, TrileanValue.True }, TrileanValue.True)]
	[TestCase(LogicalFunctionOperator.Or, new[] { TrileanValue.Unknown, TrileanValue.True }, TrileanValue.True)]
	[TestCase(LogicalFunctionOperator.Min, new[] { TrileanValue.True, TrileanValue.Unknown }, TrileanValue.Unknown)]
	[TestCase(LogicalFunctionOperator.Max, new[] { TrileanValue.Unknown, TrileanValue.False }, TrileanValue.Unknown)]
	[TestCase(LogicalFunctionOperator.IsNull, new[] { TrileanValue.True }, TrileanValue.False)]
	[TestCase(LogicalFunctionOperator.IsNotNull, new[] { TrileanValue.True }, TrileanValue.True)]
	[TestCase(LogicalFunctionOperator.IsNull, new TrileanValue[] { }, TrileanValue.True)]
	[TestCase(LogicalFunctionOperator.IsNotNull, new TrileanValue[] { }, TrileanValue.False)]
	public void TrileanTest(LogicalFunctionOperator operation, TrileanValue[] values, params TrileanValue[] results)
	{
		var fn = CreateFunction(operation, values.Select(e => (Trilean)e));

		CheckResults(fn.Execute(), results);
	}

	[TestCase(LogicalFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	[TestCase(LogicalFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(LogicalFunctionOperator.Or, new[] { false, true }, true)]
	public void TrileanTest2(LogicalFunctionOperator operation, bool[] values, params bool[] results)
	{
		var fn = CreateFunction(operation, values.Select(e => (Trilean)e));

		CheckResults(fn.Execute(), results);
	}
}