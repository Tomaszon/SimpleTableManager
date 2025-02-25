namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class DateFunctionTests : TestBase
{
	[TestCase(DateTimeFunctionOperator.Const, new[] { "0001-01-01", "0001-02-03" }, "0001-01-01", "0001-02-03")]
	[TestCase(DateTimeFunctionOperator.Avg, new[] { "0001-01-31", "0001-02-02" }, "0001-02-01")]
	public void DateOnlyTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateType.Parse(s, null)));

		CheckResults(fn.Execute(), results.Select(s => DateType.Parse(s, null)));
	}

	[TestCase(DateTimeFunctionOperator.Sub, new[] { "2025-02-23", "2025-01-20" }, "34")]
	public void DateOnlyTest2(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateType.Parse(s, null)));

		CheckResults(fn.Execute(), results.Select(s => TimeSpanType.Parse(s, null)));
	}

	[TestCase(1993, 12, 19, "yyyy.MM.dd", "1993.12.19")]
	[TestCase(1993, 12, 19, "yyyy-MM-dd", "1993-12-19")]
	public void DateOnlyTest3(int y, int m, int d, string format, string expectedResult)
	{
		var fn = CreateFunction(DateTimeFunctionOperator.Const, [new ConstFunctionArgument<DateType>(ArgumentName.Format, format)], new DateType(y, m, d));

		var formattedResult = fn.ExecuteAndFormat();

		CheckResults(formattedResult, expectedResult.Wrap());
	}

	[TestCase(DateTimeFunctionOperator.Const, typeof(DateType))]
	[TestCase(DateTimeFunctionOperator.Sub, typeof(TimeSpanType))]
	[TestCase(DateTimeFunctionOperator.Days, typeof(int))]
	public void DateOnlyTest4(DateTimeFunctionOperator operation, Type result)
	{
		var fn = CreateFunction<DateType>(operation);

		CheckResult(fn.GetOutType(), result);
	}

	[Test]
	public void DateOnlyTest5()
	{
		var fn = CreateFunction(DateTimeFunctionOperator.Offset, [new ConstFunctionArgument<DateType>(ArgumentName.Offset, "3")], new DateType(1993, 12, 19));

		CheckResults(fn.Execute(), [new DateType(1993, 12, 22)]);
	}

	[TestCase(DateTimeFunctionOperator.Years, new[] { "1993.12.19" }, 1993)]
	[TestCase(DateTimeFunctionOperator.Months, new[] { "1993.12.19" }, 12)]
	[TestCase(DateTimeFunctionOperator.Days, new[] { "1993.12.19" }, 19)]
	[TestCase(DateTimeFunctionOperator.Hours, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.Minutes, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.Seconds, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.Milliseconds, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.TotalHours, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.TotalMinutes, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.TotalSeconds, new[] { "1993.12.19" }, 0)]
	[TestCase(DateTimeFunctionOperator.TotalMilliseconds, new[] { "1993.12.19" }, 0)]
	public void DateOnlyTest6(DateTimeFunctionOperator operation, string[] values, params double[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateType.Parse(s, null)));

		CheckResults(fn.Execute(), results);
	}
}