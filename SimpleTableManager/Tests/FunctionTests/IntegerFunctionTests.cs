namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class IntegerFunctionTests : TestBase
{
	[TestCase(NumericFunctionOperator.Sum, new long[] { 4, 3 }, 7)]
	[TestCase(NumericFunctionOperator.Sub, new long[] { 4, 3 }, 1)]
	[TestCase(NumericFunctionOperator.And, new long[] { 2, 3 }, 2)]
	[TestCase(NumericFunctionOperator.Or, new long[] { 4, 2 }, 6)]
	[TestCase(NumericFunctionOperator.Neg, new long[] { 5 }, -5)]
	[TestCase(NumericFunctionOperator.Div, new long[] { 10, 5 }, 2)]
	[TestCase(NumericFunctionOperator.Min, new long[] { 1, 5, 3 }, 1)]
	[TestCase(NumericFunctionOperator.Max, new long[] { 1, 5, 3 }, 5)]
	[TestCase(NumericFunctionOperator.Abs, new long[] { -1 }, 1)]
	[TestCase(NumericFunctionOperator.Rem, new long[] { 10, 3 }, new long[] { 0, 1 })]
	[TestCase(NumericFunctionOperator.Sqrt, new long[] { 4, 9, 16 }, new long[] { 2, 3, 4 })]
	[TestCase(NumericFunctionOperator.LogN, new long[] { 2, 4, 8 }, new long[] { 1, 2, 3 })]
	[TestCase(NumericFunctionOperator.Log2, new long[] { 2, 4, 8 }, new long[] { 1, 2, 3 })]
	[TestCase(NumericFunctionOperator.Log10, new long[] { 10, 100, 1 }, new long[] { 1, 2, 0 })]
	[TestCase(NumericFunctionOperator.Pow, new long[] { 4, 3 }, new long[] { 16, 9 })]
	[TestCase(NumericFunctionOperator.Avg, new long[] { 4, 2 }, 3)]
	[TestCase(NumericFunctionOperator.Mul, new long[] { 4, 2, 3 }, 24)]
	public void IntegerTest1(NumericFunctionOperator operation, long[] values, params long[] results)
	{
		var fn = CreateFunction(operation, [new NamedConstFunctionArgument(ArgumentName.Power, 2)], values);

		CheckResults(fn.Execute(), results);
	}

	[TestCase(NumericFunctionOperator.Greater, new long[] { 4, 3 }, true)]
	[TestCase(NumericFunctionOperator.Greater, new long[] { 2, 3 }, false)]
	[TestCase(NumericFunctionOperator.Less, new long[] { 2, 3 }, true)]
	[TestCase(NumericFunctionOperator.Less, new long[] { 4, 3 }, false)]
	[TestCase(NumericFunctionOperator.GreaterOrEquals, new long[] { 4, 3 }, true)]
	[TestCase(NumericFunctionOperator.GreaterOrEquals, new long[] { 3, 3 }, true)]
	[TestCase(NumericFunctionOperator.GreaterOrEquals, new long[] { 2, 3 }, false)]
	[TestCase(NumericFunctionOperator.LessOrEquals, new long[] { 2, 3 }, true)]
	[TestCase(NumericFunctionOperator.LessOrEquals, new long[] { 3, 3 }, true)]
	[TestCase(NumericFunctionOperator.LessOrEquals, new long[] { 4, 3 }, false)]
	[TestCase(NumericFunctionOperator.Equals, new long[] { 3, 3 }, true)]
	[TestCase(NumericFunctionOperator.Equals, new long[] { 2, 3 }, false)]
	[TestCase(NumericFunctionOperator.NotEquals, new long[] { 3, 3 }, false)]
	[TestCase(NumericFunctionOperator.NotEquals, new long[] { 2, 3 }, true)]
	public void IntegerTest2(NumericFunctionOperator operation, long[] values, bool result)
	{
		var fn = CreateFunction(operation, values);

		CheckResult(fn.Execute().Single(), (FormattableBoolean)result);
	}

	[TestCase(NumericFunctionOperator.Greater, new long[] { 4, 3 }, false)]
	[TestCase(NumericFunctionOperator.Greater, new long[] { 4, 4 }, true)]
	[TestCase(NumericFunctionOperator.Less, new long[] { 2, 3 }, false)]
	[TestCase(NumericFunctionOperator.Less, new long[] { 4, 5 }, false)]
	[TestCase(NumericFunctionOperator.GreaterOrEquals, new long[] { 3, 2 }, false)]
	[TestCase(NumericFunctionOperator.GreaterOrEquals, new long[] { 3, 3 }, true)]
	[TestCase(NumericFunctionOperator.GreaterOrEquals, new long[] { 4, 3 }, true)]
	[TestCase(NumericFunctionOperator.LessOrEquals, new long[] { 4, 3 }, false)]
	[TestCase(NumericFunctionOperator.LessOrEquals, new long[] { 3, 3 }, true)]
	[TestCase(NumericFunctionOperator.LessOrEquals, new long[] { 2, 3 }, true)]
	[TestCase(NumericFunctionOperator.Equals, new long[] { 3, 3 }, true)]
	[TestCase(NumericFunctionOperator.Equals, new long[] { 2, 3 }, false)]
	[TestCase(NumericFunctionOperator.NotEquals, new long[] { 3, 3 }, false)]
	[TestCase(NumericFunctionOperator.NotEquals, new long[] { 2, 4 }, true)]
	public void IntegerTest3(NumericFunctionOperator operation, long[] values, bool result)
	{
		var na = new NamedConstFunctionArgument[]
		{
			new(ArgumentName.Reference, 3)
		};

		var fn = CreateFunction(operation, na, values);

		CheckResult(fn.Execute().Single(), (FormattableBoolean)result);
	}
}