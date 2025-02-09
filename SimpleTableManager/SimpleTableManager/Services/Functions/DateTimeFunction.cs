namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(DateTime))]
public class DateTimeFunction : DateTimeFunctionBase<DateTime, DateTime>
{
	protected override DateTime Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(DateTime.MinValue, (a, c) =>
			a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day).Add(c.TimeOfDay))
			.AddYears(-DateOnly.MinValue.Year).AddMonths(-DateOnly.MinValue.Month).AddDays(-DateOnly.MinValue.Day);
	}
}