namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Offset, "0")]
[FunctionMappingType(typeof(DateTime))]
public class DateTimeFunction : DateTimeFunctionBase<DateTime, IConvertible>
{
	public override IEnumerable<IConvertible> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => UnwrappedUnnamedArguments.Cast<IConvertible>(),
			DateTimeFunctionOperator.Offset => Offset().Cast<IConvertible>(),

			_ => base.ExecuteCore()
		};
	}

	// protected ConvertibleTimeSpan Sum()
	// {
	// 	// return UnwrappedUnnamedArguments.Aggregate(DateTime.MinValue, (a, c) =>
	// 	// a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day).Add(c.TimeOfDay))
	// 	// .AddYears(-DateOnly.MinValue.Year).AddMonths(-DateOnly.MinValue.Month).AddDays(-DateOnly.MinValue.Day);
	// 	throw new NotImplementedException();
	// }

	protected ConvertibleTimeSpan Sub()
	{
		throw new NotImplementedException();
	}

	protected IEnumerable<DateTime> Offset()
	{
		var offset = TimeSpan.Parse(GetNamedArgument<string>(ArgumentName.Offset));

		return UnwrappedUnnamedArguments.Select(a => a.Add(offset));
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

	protected override IEnumerable<double> TotalYears()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year + a.Month / (double)MONTHS_IN_A_YEAR + a.Day / DAYS_IN_A_MONTH);
	}

	protected override IEnumerable<double> TotalMonths()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year * MONTHS_IN_A_YEAR + a.Month + a.Day / DAYS_IN_A_MONTH);
	}

	protected override IEnumerable<double> TotalDays()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year * DAYS_IN_A_YEAR + a.Month * DAYS_IN_A_MONTH + a.Day);
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
		return UnwrappedUnnamedArguments.Select(a => new TimeSpan(a.Ticks).TotalHours);
	}

	protected override IEnumerable<double> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => new TimeSpan(a.Ticks).TotalMinutes);
	}
	protected override IEnumerable<double> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => new TimeSpan(a.Ticks).TotalSeconds);
	}
	protected override IEnumerable<double> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => new TimeSpan(a.Ticks).TotalMilliseconds);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => typeof(DateTime),

			_ => base.GetOutType()
		};
	}
}