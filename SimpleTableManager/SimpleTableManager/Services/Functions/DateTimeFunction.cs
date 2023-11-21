namespace SimpleTableManager.Services.Functions;

public class DateTimeFunction : DateTimeFunctionBase<DateTime, DateTime, DateTime>
{
	protected override DateTime Sum()
	{
		return Arguments.Aggregate(DateTime.MinValue, (a, c) => a.Add(new TimeSpan(c.Ticks)));
	}

	protected override DateTime Now()
	{
		return NowProperty is null ? (NowProperty = DateTime.Now).Value : NowProperty.Value;
	}
}