namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Offset, "0")]
[FunctionMappingType(typeof(DateTimeType))]
public class DateTimeFunction : DateTimeFunctionBase<DateTimeType, IType>
{
	public override IEnumerable<IType> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => UnwrappedUnnamedArguments,
			DateTimeFunctionOperator.Offset => Offset(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),
			DateTimeFunctionOperator.TotalDays => throw GetInvalidOperatorException(),

			_ => base.ExecuteCore()
		};
	}

	protected TimeSpanType Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(new TimeSpan(UnwrappedUnnamedArguments.First().Ticks), (a, c) => a.Subtract(new TimeSpan(c.Ticks)));
	}

	protected IEnumerable<DateTimeType> Offset()
	{
		var offset = TimeSpan.Parse(GetNamedArgument<string>(ArgumentName.Offset));

		return UnwrappedUnnamedArguments.Select(a => a.Add(offset));
	}

	protected override DateTimeType Avg()
	{
		var a = UnwrappedUnnamedArguments.Aggregate(DateTime.MinValue, (a, c) => a.AddTicks(c.Ticks));

		return new DateTime(a.Ticks / UnwrappedUnnamedArguments.Count());
	}

	protected override IEnumerable<IntegerType> Years()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year);
	}

	protected override IEnumerable<IntegerType> Months()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Month);
	}

	protected override IEnumerable<IntegerType> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Day);
	}

	protected override IEnumerable<IntegerType> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Hour);
	}

	protected override IEnumerable<IntegerType> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Minute);
	}

	protected override IEnumerable<IntegerType> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Second);
	}

	protected override IEnumerable<IntegerType> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Millisecond);
	}

	protected override IEnumerable<FractionType> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalHours);
	}

	protected override IEnumerable<FractionType> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalMinutes);
	}
	protected override IEnumerable<FractionType> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalSeconds);
	}
	protected override IEnumerable<FractionType> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => a.TimeOfDay.TotalMilliseconds);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Avg or
			DateTimeFunctionOperator.Now => typeof(DateTime),

			_ => base.GetOutType()
		};
	}
}