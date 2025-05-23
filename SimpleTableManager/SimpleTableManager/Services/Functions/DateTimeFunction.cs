using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Offset, "0")]
[FunctionMappingType(typeof(DateTime))]
public class DateTimeFunction : DateTimeFunctionBase<DateTime>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Yesterday or
			DateTimeFunctionOperator.Tomorrow => UnwrappedUnnamedArguments.Cast<object>(),
			DateTimeFunctionOperator.Offset => Offset().Cast<object>(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),
			DateTimeFunctionOperator.Min => Min(DateTime.MinValue).Wrap(),
			DateTimeFunctionOperator.Max => Max(DateTime.MaxValue).Wrap(),

			_ => base.ExecuteCore()
		};
	}

	protected ConvertibleTimeSpan Sub()
	{
		return UnwrappedUnnamedArgumentsIfAny(TimeSpan.Zero, () => UnwrappedUnnamedArguments.Skip(1).Aggregate(new TimeSpan(UnwrappedUnnamedArguments.First().Ticks), (a, c) => a.Subtract(new TimeSpan(c.Ticks))));
	}

	protected IEnumerable<DateTime> Offset()
	{
		var offset = TimeSpan.Parse(GetNamedArgument<string>(ArgumentName.Offset));

		return UnwrappedUnnamedArguments.Select(a => a.Add(offset));
	}

	protected override object Avg()
	{
		return UnwrappedUnnamedArgumentsIfAny(DateTime.MinValue, () => new DateTime(UnwrappedUnnamedArguments.Aggregate(DateTime.MinValue, (a, c) => new DateTime(a.Ticks + c.Ticks)).Ticks / UnwrappedUnnamedArguments.Count()));
	}

	protected override IEnumerable<int> Years()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year);
	}

	protected override IEnumerable<int> Months()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Month);
	}

	protected override IEnumerable<int> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Day);
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
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalHours);
	}

	protected override IEnumerable<double> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalMinutes);
	}
	protected override IEnumerable<double> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalSeconds);
	}
	protected override IEnumerable<double> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalMilliseconds);
	}
}