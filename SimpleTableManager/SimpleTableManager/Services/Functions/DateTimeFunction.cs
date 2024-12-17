namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(DateTime))]
public class DateTimeFunction : DateTimeFunctionBase<DateTime, DateTime>
{
	protected override DateTime Sum()
	{
		return ConvertedUnwrappedArguments.Aggregate(DateTime.MinValue, (a, c) => a.Add(new TimeSpan(c.Ticks)));
	}

	protected override DateTime Now()
	{
		return NowProperty is null ? (NowProperty = DateTime.Now).Value : NowProperty.Value;
	}
}