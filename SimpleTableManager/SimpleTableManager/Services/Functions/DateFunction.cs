namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(ConvertibleDateOnly))]
public class DateFunction : DateTimeFunctionBase<ConvertibleDateOnly, ConvertibleDateOnly>
{
	protected override ConvertibleDateOnly Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(DateOnly.MinValue, (a, c) => 
			a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day))
			.AddYears(-DateOnly.MinValue.Year).AddMonths(-DateOnly.MinValue.Month).AddDays(-DateOnly.MinValue.Day);
	}
}