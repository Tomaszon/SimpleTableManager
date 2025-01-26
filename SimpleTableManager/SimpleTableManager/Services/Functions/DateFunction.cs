namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(ConvertibleDateOnly))]
public class DateFunction : NullableDateTimeFunctionBase<ConvertibleDateOnly, ConvertibleDateOnly>
{
	public override string GetFriendlyName()
	{
		return typeof(DateOnly).GetFriendlyName();
	}

	protected override ConvertibleDateOnly Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(DateOnly.MinValue, (a, c) => 
			a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day))
			.AddYears(-DateOnly.MinValue.Year).AddMonths(-DateOnly.MinValue.Month).AddDays(-DateOnly.MinValue.Day);
	}

	protected override ConvertibleDateOnly Now()
	{
		if (NowProperty is null)
		{
			(var d, _) = DateTime.Now;

			NowProperty = d;
		}

		return NowProperty;
	}
}