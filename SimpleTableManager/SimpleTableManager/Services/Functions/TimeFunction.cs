namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(TimeOnly))]
public class TimeFunction : DateTimeFunctionBase<TimeOnly, TimeOnly>
{
	public override string GetFriendlyName()
	{
		return typeof(TimeOnly).GetFriendlyName();
	}

	protected override TimeOnly Sum()
	{
		return UnwrappedArguments.Aggregate(TimeOnly.MinValue, (a, c) => a.Add(c.ToTimeSpan()));
	}

	protected override TimeOnly Now()
	{
		if (NowProperty is null)
		{
			(_, TimeOnly t) = DateTime.Now;

			NowProperty = t;
		}

		return NowProperty.Value;
	}
}