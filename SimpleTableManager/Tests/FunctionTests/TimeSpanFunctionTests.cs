namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class TimeSpanFunctionTests : TestBase
{
	[TestCase(DateTimeFunctionOperator.Const, new[] { "1.12:50:01", "10:51:02" }, new[] { "1.12:50:01", "10:51:02" })]
	[TestCase(DateTimeFunctionOperator.Sum, new[] { "1.12:50:01", "10:51:02" }, "1.23:41:03")]
	[TestCase(DateTimeFunctionOperator.Sub, new[] { "1.23:41:03", "10:51:02" }, "1.12:50:01")]
	[TestCase(DateTimeFunctionOperator.Avg, new[] { "1.01:01:01", "3.03:03:03" }, "2.02:02:02")]
	public void TimeSpanTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => TimeSpanType.Parse(s, null)));

		CheckResults(fn.Execute(), results.Select(s => TimeSpanType.Parse(s, null)));
	}

	[TestCase(DateTimeFunctionOperator.Days, new[] { "1.02:03:04.005" }, 1)]
	[TestCase(DateTimeFunctionOperator.Hours, new[] { "1.02:03:04.005" }, 2)]
	[TestCase(DateTimeFunctionOperator.Minutes, new[] { "1.02:03:04.005" }, 3)]
	[TestCase(DateTimeFunctionOperator.Seconds, new[] { "1.02:03:04.005" }, 4)]
	[TestCase(DateTimeFunctionOperator.Milliseconds, new[] { "1.02:03:04.005" }, 5)]
	[TestCase(DateTimeFunctionOperator.TotalDays, new[] { "1.02:03:04.005" }, 1.0854630208333333)]
	[TestCase(DateTimeFunctionOperator.TotalHours, new[] { "1.02:03:04.005" }, 26.0511125)]
	[TestCase(DateTimeFunctionOperator.TotalMinutes, new[] { "1.02:03:04.005" }, 1_563.06675)]
	[TestCase(DateTimeFunctionOperator.TotalSeconds, new[] { "1.02:03:04.005" }, 93_784.005)]
	[TestCase(DateTimeFunctionOperator.TotalMilliseconds, new[] { "1.02:03:04.005" }, 93_784_005)]
	public void TimeSpanTest2(DateTimeFunctionOperator operation, string[] values, params double[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => TimeSpanType.Parse(s, null)));

		CheckResults(fn.Execute(), results.CastTo<FractionType>());
	}

	[TestCase(DateTimeFunctionOperator.Const, typeof(TimeSpanType))]
	[TestCase(DateTimeFunctionOperator.Sub, typeof(TimeSpanType))]
	[TestCase(DateTimeFunctionOperator.Days, typeof(int))]
	[TestCase(DateTimeFunctionOperator.TotalDays, typeof(double))]
	public void TimeSpanTest3(DateTimeFunctionOperator operation, Type result)
	{
		var fn = CreateFunction<TimeSpanType>(operation);

		CheckResult(fn.GetOutType(), result);
	}
}