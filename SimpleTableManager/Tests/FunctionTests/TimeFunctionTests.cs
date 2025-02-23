namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class TimeFunctionTests : TestBase
{
    [TestCase(DateTimeFunctionOperator.Const, new[] { "01:02:03.001", "01:02:03.1", "01:02:03", "01:02" }, "01:02:03.001", "01:02:03.1", "01:02:03", "01:02")]
    public void TimeOnlyTest1(DateTimeFunctionOperator operation, string[] values, params string[] results)
    {
        var fn = CreateFunction(operation, values.Select(s => ConvertibleTimeOnly.Parse(s, null)));

        CheckResults(fn.Execute(), results.Select(s => ConvertibleTimeOnly.Parse(s, null)));
    }

    [TestCase(DateTimeFunctionOperator.Sub, new[] { "06:05:04.003", "01:01:01.001" }, "05:04:03.002")]
    public void TimeOnlyTest2(DateTimeFunctionOperator operation, string[] values, params string[] results)
    {
        var fn = CreateFunction(operation, values.Select(s => ConvertibleTimeOnly.Parse(s, null)));

        CheckResults(fn.Execute(), results.Select(s => ConvertibleTimeSpan.Parse(s, null)));
    }

    [TestCase(10, 5, 2, "HH:mm:ss", "10:05:02")]
    [TestCase(10, 0, 0, "HH", "10")]
    public void TimeOnlyTest3(int h, int m, int s, string format, string expectedResult)
    {
        var fn = CreateFunction(DateTimeFunctionOperator.Const, [new ConstFunctionArgument<ConvertibleTimeOnly>(ArgumentName.Format, format)], new ConvertibleTimeOnly(h, m, s));

        var formattedResult = fn.ExecuteAndFormat();

        CheckResults(formattedResult, expectedResult.Wrap());
    }

    [TestCase(DateTimeFunctionOperator.Hours, new[] { "02:03:04.005" }, 2)]
    [TestCase(DateTimeFunctionOperator.Minutes, new[] { "02:03:04.005" }, 3)]
    [TestCase(DateTimeFunctionOperator.Seconds, new[] { "02:03:04.005" }, 4)]
    [TestCase(DateTimeFunctionOperator.Milliseconds, new[] { "02:03:04.005" }, 5)]
    [TestCase(DateTimeFunctionOperator.TotalHours, new[] { "02:03:04.005" }, 2.0511125)]
    [TestCase(DateTimeFunctionOperator.TotalMinutes, new[] { "02:03:04.005" }, 123.06675)]
    [TestCase(DateTimeFunctionOperator.TotalSeconds, new[] { "02:03:04.005" }, 7_384.005)]
    [TestCase(DateTimeFunctionOperator.TotalMilliseconds, new[] { "02:03:04.005" }, 7_384_005)]
    public void TimeOnlyTest4(DateTimeFunctionOperator operation, string[] values, params double[] results)
    {
        var fn = CreateFunction(operation, values.Select(s => ConvertibleTimeOnly.Parse(s, null)));

        CheckResults(fn.Execute(), results);
    }

    [TestCase(DateTimeFunctionOperator.Const, typeof(ConvertibleTimeOnly))]
    [TestCase(DateTimeFunctionOperator.Now, typeof(ConvertibleTimeOnly))]
    [TestCase(DateTimeFunctionOperator.Days, typeof(int))]
    [TestCase(DateTimeFunctionOperator.TotalDays, typeof(double))]
    public void TimeOnlyTest5(DateTimeFunctionOperator operation, Type result)
    {
        var fn = CreateFunction<ConvertibleTimeOnly>(operation);

        CheckResult(fn.GetOutType(), expectedValue: result);
    }

    [TestCase(DateTimeFunctionOperator.Offset, new[] { "01:02:03.001", "01:02:03.1", "01:02:03", "01:02" }, "02:04:06.001", "02:04:06.1", "02:04:06", "02:04:03")]
    public void TimeOnlyTest6(DateTimeFunctionOperator operation, string[] values, params string[] results)
    {
        var fn = CreateFunction(operation, [new ConstFunctionArgument<string>(ArgumentName.Offset, "01:02:03")], values.Select(s => ConvertibleTimeOnly.Parse(s, null)));

        CheckResults(fn.Execute(), results.Select(s => ConvertibleTimeOnly.Parse(s, null)));
    }
}