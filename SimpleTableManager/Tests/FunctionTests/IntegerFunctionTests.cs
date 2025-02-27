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
	public void IntegerTest(NumericFunctionOperator operation, long[] values, params long[] results)
	{
		var na = new IConstFunctionArgument[]
		{
			new ConstFunctionArgument<IntegerType>(ArgumentName.Power, 2)
		};

		var fn = CreateFunction(operation, na, values.CastTo<IntegerType>());

		CheckResults(fn.Execute(), results.CastTo<FractionType>());
	}
}