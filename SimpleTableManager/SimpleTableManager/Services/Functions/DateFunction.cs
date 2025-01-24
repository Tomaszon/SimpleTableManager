// namespace SimpleTableManager.Services.Functions;

// [FunctionMappingType(typeof(ConvertibleDateOnly))]
// public class DateFunction : NullableDateTimeFunctionBase<ConvertibleDateOnly, ConvertibleDateOnly>
// {
// 	public override string GetFriendlyName()
// 	{
// 		return typeof(ConvertibleDateOnly).GetFriendlyName();
// 	}

// 	protected override ConvertibleDateOnly Sum()
// 	{
// 		return null;//UnwrappedUnnamedArguments.Aggregate(DateOnly.MinValue, (a, c) =>
// 			//a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day));
// 	}

// 	protected override ConvertibleDateOnly Now()
// 	{
// 		if (NowProperty is null)
// 		{
// 			(var d, _) = DateTime.Now;

// 			NowProperty = d;
// 		}

// 		return NowProperty;
// 	}
// }