namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class FractionFunctionTests : TestBase
{
	[Test]
	public void FractionTest1()
	{
		var fn = CreateFunction(NumericFunctionOperator.Sum, [4d, 3d]);

		CheckResults(fn.Execute(), 7d.Wrap());
	}

	[Test]
	public void FractionTest2()
	{
		var fn = CreateFunction(NumericFunctionOperator.Sqrt, [4d, 9d, 16d]);

		CheckResults(fn.Execute(), [2d, 3d, Math.Sqrt(16d)]);
	}

	[Test]
	public void FractionTest3()
	{
		var fn = CreateFunction(NumericFunctionOperator.LogN, [2d, 4d, 16d]);

		CheckResults(fn.Execute(), [1d, 2d, Math.Log2(16d)]);
	}

	[Test]
	public void FractionTest4()
	{
		var fn = CreateFunction(NumericFunctionOperator.LogE, [double.E, double.E * double.E]);

		CheckResults(fn.Execute(), [1d, 2d]);
	}

	[TestCase(NumericFunctionOperator.Round, 4.234, 4.23, typeof(double))]
	[TestCase(NumericFunctionOperator.Round, 4.235, 4.24, typeof(double))]
	[TestCase(NumericFunctionOperator.Ceiling, 4.235, 5, typeof(long))]
	[TestCase(NumericFunctionOperator.Floor, 4.235, 4, typeof(long))]
	public void FractionTest5(NumericFunctionOperator functionOperator, double number, double expected, Type expectedResultType)
	{
		var fn = CreateFunction(functionOperator, [number]);

		CheckResults(fn.Execute(), [expected]);
		CheckResult(fn.GetOutType(), expectedResultType);
	}

	[TestCase(NumericFunctionOperator.Greater, 4.235, 4, true, typeof(FormattableBoolean))]
	public void FractionTest6(NumericFunctionOperator functionOperator, double number1, double number2, bool result, Type expectedResultType)
	{
		var fn = CreateFunction(functionOperator, [number1, number2]);

		CheckResults(fn.Execute(), [(FormattableBoolean)result]);
		CheckResult(fn.GetOutType(), expectedResultType);
	}

	[TestCase(NumericFunctionOperator.Max, new[] { 4.235, 4 }, 4.235, typeof(double))]
	[TestCase(NumericFunctionOperator.Min, new[] { 4.235, 4 }, 4, typeof(double))]
	public void FractionTest6(NumericFunctionOperator functionOperator, double[] numbers, double expected, Type expectedResultType)
	{
		var fn = CreateFunction(functionOperator, numbers);

		CheckResults(fn.Execute(), [expected]);
		CheckResult(fn.GetOutType(), expectedResultType);
	}
}