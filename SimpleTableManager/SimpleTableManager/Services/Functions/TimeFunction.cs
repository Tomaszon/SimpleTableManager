namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Offset, "0")]
[FunctionMappingType(typeof(TimeType))]
public class TimeFunction : DateTimeFunctionBase<TimeType, IType>
{
	public override IEnumerable<IType> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => UnwrappedUnnamedArguments,
			DateTimeFunctionOperator.Offset => Offset(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),

			_ => base.ExecuteCore()
		};
	}
	// protected ConvertibleTimeSpan Sum()
	// {
	// 	// return UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue.ToTimeSpan(), (a, c) => a += c.ToTimeSpan());
	// 	throw new NotImplementedException();
	// }

	protected TimeSpanType Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(((TimeOnly)UnwrappedUnnamedArguments.First()).ToTimeSpan(), (a, c) => a -= c.ToTimeSpan());
	}

	protected IEnumerable<TimeType> Offset()
	{
		var offset = TimeSpan.Parse(GetNamedArgument<string>(ArgumentName.Offset));

		return UnwrappedUnnamedArguments.Select(a => a.Add(offset));
	}

    protected override TimeType Avg()
    {
        return TimeOnly.FromTimeSpan(UnwrappedUnnamedArguments.Aggregate(TimeOnly.MinValue.ToTimeSpan(), (a, c) => a += c.ToTimeSpan()).Divide(UnwrappedUnnamedArguments.Count()));
    }

    protected override IEnumerable<IntegerType> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Hour);
	}

	protected override IEnumerable<IntegerType> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Minute);
	}

	protected override IEnumerable<IntegerType> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Second);
	}

	protected override IEnumerable<IntegerType> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Millisecond);
	}

	protected override IEnumerable<FractionType> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.ToTimeSpan().TotalHours);
	}

	protected override IEnumerable<FractionType> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.ToTimeSpan().TotalMinutes);
	}

	protected override IEnumerable<FractionType> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.ToTimeSpan().TotalSeconds);
	}

	protected override IEnumerable<FractionType> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.ToTimeSpan().TotalMilliseconds);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Avg or
			DateTimeFunctionOperator.Now => typeof(TimeType),

			_ => base.GetOutType()
		};
	}
}