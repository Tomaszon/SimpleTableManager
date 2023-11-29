namespace SimpleTableManager.Services.Functions;

public class DateFunction : DateTimeFunctionBase<DateOnly, DateOnly, DateOnly>
{
	protected override DateOnly Sum()
	{
		return Arguments.Aggregate(DateOnly.MinValue, (a, c) =>
			a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day));
	}

	protected override DateOnly Now()
	{
		if (NowProperty is null)
		{
			(DateOnly d, _) = DateTime.Now;

			NowProperty = d;
		}

		return NowProperty.Value;
	}
}