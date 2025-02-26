namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class FractionFunctionTests : TestBase
{
	[Test]
	public void FractionTest1()
	{
		var fn = CreateFunction<FractionType>(NumericFunctionOperator.Sum, 4, 3);

		CheckResults(fn.Execute(), 7d.Wrap());
	}

	[Test]
	public void FractionTest2()
	{
		var fn = CreateFunction<FractionType>(NumericFunctionOperator.Sqrt, 4, 9, 16);

		CheckResults(fn.Execute(), [2d, 3d, Math.Sqrt(16d)]);
	}

	[Test]
	public void FractionTest3()
	{
		var fn = CreateFunction<FractionType>(NumericFunctionOperator.LogN, 2, 4, 16);

		CheckResults(fn.Execute(), [1d, 2d, Math.Log2(16d)]);
	}

	[Test]
	public void FractionTest4()
	{
		var fn = CreateFunction<FractionType>(NumericFunctionOperator.LogE, double.E, double.E * double.E);

		CheckResults(fn.Execute(), [1d, 2d]);
	}

	[TestCase(NumericFunctionOperator.Round, 4.234, 4.23, typeof(double))]
	[TestCase(NumericFunctionOperator.Round, 4.235, 4.24, typeof(double))]
	[TestCase(NumericFunctionOperator.Ceiling, 4.235, 5, typeof(long))]
	[TestCase(NumericFunctionOperator.Floor, 4.235, 4, typeof(long))]
	public void FractionTest5(NumericFunctionOperator functionOperator, double number, double expected, Type expectedResultType)
	{
		var fn = CreateFunction<FractionType>(functionOperator, number);

		CheckResults(fn.Execute(), [expected]);
		CheckResult(fn.GetOutType(), expectedResultType);
	}
}