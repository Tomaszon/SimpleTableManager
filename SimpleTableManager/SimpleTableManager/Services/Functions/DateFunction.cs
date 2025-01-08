namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(DateOnly))]
public class DateFunction : DateTimeFunctionBase<DateOnly, DateOnly>
{
	public override string GetFriendlyName()
	{
		return typeof(DateOnly).GetFriendlyName();
	}

	protected override DateOnly Sum()
	{
		return UnwrappedArguments.Aggregate(DateOnly.MinValue, (a, c) =>
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