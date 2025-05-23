using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Offset, "0")]
[FunctionMappingType(typeof(ConvertibleTimeOnly))]
public class TimeFunction : DateTimeFunctionBase<ConvertibleTimeOnly>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Offset => Offset(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),
			DateTimeFunctionOperator.Min => Min(TimeOnly.MinValue).Wrap(),
			DateTimeFunctionOperator.Max => Max(TimeOnly.MaxValue).Wrap(),

			_ => base.ExecuteCore()
		};
	}
	// protected ConvertibleTimeSpan Sum()
	// {
	// 	// return UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue.ToTimeSpan(), (a, c) => a += c.ToTimeSpan());
	// 	throw new NotImplementedException();
	// }

	protected ConvertibleTimeSpan Sub()
	{
		return UnwrappedUnnamedArgumentsIfAny(TimeSpan.Zero, () => UnwrappedUnnamedArguments.Skip(1).Aggregate(((TimeOnly)UnwrappedUnnamedArguments.First()).ToTimeSpan(), (a, c) => a -= c.ToTimeSpan()));
	}

	protected IEnumerable<ConvertibleTimeOnly> Offset()
	{
		var offset = TimeSpan.Parse(GetNamedArgument<string>(ArgumentName.Offset));

		return UnwrappedUnnamedArguments.Select(a => a.Add(offset));
	}

	protected override ConvertibleTimeOnly Avg()
	{
		return UnwrappedUnnamedArgumentsIfAny(TimeOnly.MinValue, () => TimeOnly.FromTimeSpan(UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue.ToTimeSpan(), (a, c) => a += c.ToTimeSpan()).Divide(UnwrappedUnnamedArguments.Count())));
	}

	protected override IEnumerable<int> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Hour);
	}

	protected override IEnumerable<int> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Minute);
	}

	protected override IEnumerable<int> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Second);
	}

	protected override IEnumerable<int> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Millisecond);
	}

	protected override IEnumerable<double> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => a.ToTimeSpan().TotalHours);
	}

	protected override IEnumerable<double> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.ToTimeSpan().TotalMinutes);
	}

	protected override IEnumerable<double> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.ToTimeSpan().TotalSeconds);
	}

	protected override IEnumerable<double> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.ToTimeSpan().TotalMilliseconds);
	}
}