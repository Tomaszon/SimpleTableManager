namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(TimeSpanType))]
public class TimeSpanFunction : DateTimeFunctionBase<TimeSpanType, IType>
{
	public override IEnumerable<IType> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const => UnwrappedUnnamedArguments,
			DateTimeFunctionOperator.Sum => Sum().Wrap(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),

			_ => base.ExecuteCore()
		};
	}

	protected TimeSpanType Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TimeSpan.Zero, (a, c) => a + c);
	}

	protected TimeSpanType Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate((TimeSpan)UnwrappedUnnamedArguments.First(), (a, c) => a - c);
	}

	protected override TimeSpanType Avg()
	{
		return Sum().Divide(UnwrappedUnnamedArguments.Count());
	}

	protected override IEnumerable<IntegerType> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Days);
	}

	protected override IEnumerable<IntegerType> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Hours);
	}

	protected override IEnumerable<IntegerType> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Minutes);
	}

	protected override IEnumerable<IntegerType> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Seconds);
	}

	protected override IEnumerable<IntegerType> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (IntegerType)a.Milliseconds);
	}

	protected override IEnumerable<FractionType> TotalDays()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.TotalDays);
	}

	protected override IEnumerable<FractionType> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.TotalHours);
	}

	protected override IEnumerable<FractionType> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.TotalMinutes);
	}

	protected override IEnumerable<FractionType> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.TotalSeconds);
	}

	protected override IEnumerable<FractionType> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => (FractionType)a.TotalMilliseconds);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Avg => typeof(TimeSpanType),

			_ => base.GetOutType()
		};
	}
}
