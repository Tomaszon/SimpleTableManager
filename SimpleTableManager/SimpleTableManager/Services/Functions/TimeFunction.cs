namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(ConvertibleTimeOnly))]
public class TimeFunction : NullableDateTimeFunctionBase<ConvertibleTimeOnly, ConvertibleTimeOnly>
{
	public override string GetFriendlyName()
	{
		return typeof(TimeOnly).GetFriendlyName();
	}

	protected override ConvertibleTimeOnly Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue, (a, c) => a.Add(c.ToTimeSpan()));
	}

	protected override ConvertibleTimeOnly Now()
	{
		if (NowProperty is null)
		{
			(_, var t) = DateTime.Now;

			NowProperty = t;
		}

		return NowProperty;
	}
}