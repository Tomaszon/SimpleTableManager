namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(ConvertibleTimeOnly))]
public class TimeFunction : DateTimeFunctionBase<ConvertibleTimeOnly, ConvertibleTimeOnly>
{
	public override string GetFriendlyName()
	{
		return typeof(ConvertibleTimeOnly).GetFriendlyName();
	}

	protected override ConvertibleTimeOnly Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue, (a, c) => a.Add(c.ToTimeSpan()));
	}
}