namespace SimpleTableManager.Services.Functions;

[NamedArgument(ArgumentName.Format, "yyyy.MM.dd-HH:mm:ss")]
public class DateTimeFunction : FunctionBase<DateTimeFunctionOperator, DateTime, DateTime>
{
	protected override IEnumerable<DateTime> Execute()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const => Arguments,
			DateTimeFunctionOperator.Sum => Sum().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private DateTime Sum()
	{
		return Arguments.Aggregate(DateTime.MinValue, (a, c) => 
			a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day)
			.AddHours(c.Hour).AddMinutes(c.Minute).AddSeconds(c.Second)
			.AddMilliseconds(c.Millisecond).AddMicroseconds(c.Microsecond))
			.AddYears(-1).AddMonths(-1).AddDays(-1);
	}
}