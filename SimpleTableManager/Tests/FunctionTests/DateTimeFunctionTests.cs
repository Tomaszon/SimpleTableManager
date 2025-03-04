namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class DateTimeFunctionTests : TestBase
{
	[TestCase(DateTimeFunctionOperator.Const, new[] { "0002-02-02 10:30", "0002-02-02 02:20" }, "0002-02-02 10:30", "0002-02-02 02:20")]
	[TestCase(DateTimeFunctionOperator.Avg, new[] { "0001-01-31 02:02", "0001-02-02 03:03" }, "0001-02-01 02:32:30")]
	public void DateTimeTest1(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s)));

		CheckResults(fn.Execute(), results.Select(s => DateTime.Parse(s)));
	}

	[TestCase(DateTimeFunctionOperator.Sub, new[] { "0005-03-05 10:30", "0005-03-04 02:20" }, "1.08:10")]
	public void DateTimeTest2(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s)));

		CheckResults(fn.Execute(), results.Select(s => ConvertibleTimeSpan.Parse(s, null)));
	}


	[TestCase(DateTimeFunctionOperator.Const, typeof(DateTime))]
	[TestCase(DateTimeFunctionOperator.Sub, typeof(ConvertibleTimeSpan))]
	[TestCase(DateTimeFunctionOperator.Days, typeof(int))]
	public void DateTimeTest3(DateTimeFunctionOperator operation, Type result)
	{
		var fn = CreateFunction<DateTime>(operation);

		CheckResult(fn.GetOutType(), result);
	}

	[Test]
	public void DateTimeTest4()
	{
		var fn = CreateFunction(DateTimeFunctionOperator.Offset, [new NamedConstFunctionArgument(ArgumentName.Offset, "3.12:30:15")], new DateTime(1993, 12, 19));

		CheckResults(fn.Execute(), [new DateTime(1993, 12, 22, 12, 30, 15)]);
	}

	[TestCase(DateTimeFunctionOperator.Years, new[] { "1993.12.19" }, 1993)]
	[TestCase(DateTimeFunctionOperator.Months, new[] { "1993.12.19" }, 12)]
	[TestCase(DateTimeFunctionOperator.Days, new[] { "1993.12.19" }, 19)]
	[TestCase(DateTimeFunctionOperator.Hours, new[] { "1993.12.19 12:30:15" }, 12)]
	[TestCase(DateTimeFunctionOperator.Minutes, new[] { "1993.12.19 12:30:15" }, 30)]
	[TestCase(DateTimeFunctionOperator.Seconds, new[] { "1993.12.19 12:30:15" }, 15)]
	[TestCase(DateTimeFunctionOperator.Milliseconds, new[] { "1993.12.19 12:30:15.500" }, 500)]
	[TestCase(DateTimeFunctionOperator.TotalHours, new[] { "1993.12.19 02:03:04.005" }, 2.0511125)]
	[TestCase(DateTimeFunctionOperator.TotalMinutes, new[] { "1993.12.19 02:03:04.005" }, 123.06675)]
	[TestCase(DateTimeFunctionOperator.TotalSeconds, new[] { "1993.12.19 02:03:04.005" }, 7_384.005)]
	[TestCase(DateTimeFunctionOperator.TotalMilliseconds, new[] { "1993.12.19 02:03:04.005" }, 7_384_005)]

	public void DateTimeTest5(DateTimeFunctionOperator operation, string[] values, params double[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s, null)));

		CheckResults(fn.Execute(), results);
	}
}