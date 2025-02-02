namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(ConvertibleTimeOnly))]
public class TimeFunction : DateTimeFunctionBase<ConvertibleTimeOnly, ConvertibleTimeOnly>
{
	protected override ConvertibleTimeOnly Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue, (a, c) => a.Add(c.ToTimeSpan()));
	}
}