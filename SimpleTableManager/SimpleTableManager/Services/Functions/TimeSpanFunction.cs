namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(ConvertibleTimeSpan))]
public class TimeSpanFunction : DateTimeFunctionBase<ConvertibleTimeSpan, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const => UnwrappedUnnamedArguments,
			DateTimeFunctionOperator.Sum => Sum().Wrap(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),

			_ => base.ExecuteCore()
		};
	}

	protected ConvertibleTimeSpan Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TimeSpan.Zero, (a, c) => a + c);
	}

	protected ConvertibleTimeSpan Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate((TimeSpan)UnwrappedUnnamedArguments.First(), (a, c) => a - c);
	}

	protected override ConvertibleTimeSpan Avg()
	{
		return Sum().Divide(UnwrappedUnnamedArguments.Count());
	}

	protected override IEnumerable<int> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Days);
	}

	protected override IEnumerable<int> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Hours);
	}

	protected override IEnumerable<int> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Minutes);
	}

	protected override IEnumerable<int> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Seconds);
	}

	protected override IEnumerable<int> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Milliseconds);
	}

	protected override IEnumerable<double> TotalDays()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TotalDays);
	}

	protected override IEnumerable<double> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TotalHours);
	}

	protected override IEnumerable<double> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TotalMinutes);
	}

	protected override IEnumerable<double> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TotalSeconds);
	}

	protected override IEnumerable<double> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TotalMilliseconds);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Avg => typeof(ConvertibleTimeSpan),

			_ => base.GetOutType()
		};
	}
}
