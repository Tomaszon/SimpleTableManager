namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class DateTimeFunctionTests : TestBase
{
	[TestCase(DateTimeFunctionOperator.Const, new[] { "0002-02-02 10:30", "0002-02-02 02:20" }, "0002-02-02 10:30", "0002-02-02 02:20")]
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
}