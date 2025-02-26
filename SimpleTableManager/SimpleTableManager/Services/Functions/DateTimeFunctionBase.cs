namespace SimpleTableManager.Services.Functions;

public abstract class DateTimeFunctionBase<TIn, TOut> :
	FunctionBase<DateTimeFunctionOperator, TIn, TOut>
	where TIn : IType
	where TOut : IType
{
	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Avg => Avg().Wrap(),
			DateTimeFunctionOperator.Years => Years().Cast<TOut>(),
			DateTimeFunctionOperator.Months => Months().Cast<TOut>(),
			DateTimeFunctionOperator.Days => Days().Cast<TOut>(),
			DateTimeFunctionOperator.Hours => Hours().Cast<TOut>(),
			DateTimeFunctionOperator.Minutes => Minutes().Cast<TOut>(),
			DateTimeFunctionOperator.Seconds => Seconds().Cast<TOut>(),
			DateTimeFunctionOperator.Milliseconds => Milliseconds().Cast<TOut>(),
			DateTimeFunctionOperator.TotalDays => TotalDays().Cast<TOut>(),
			DateTimeFunctionOperator.TotalHours => TotalHours().Cast<TOut>(),
			DateTimeFunctionOperator.TotalMinutes => TotalMinutes().Cast<TOut>(),
			DateTimeFunctionOperator.TotalSeconds => TotalSeconds().Cast<TOut>(),
			DateTimeFunctionOperator.TotalMilliseconds => TotalMilliseconds().Cast<TOut>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected abstract TOut Avg();

	protected virtual IEnumerable<IntegerType> Years()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<IntegerType> Months()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<IntegerType> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<FractionType> TotalDays()
	{
		return UnwrappedUnnamedArguments.Select(a =>FractionType.Zero);
	}

	protected virtual IEnumerable<IntegerType> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<FractionType> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => FractionType.Zero);
	}

	protected virtual IEnumerable<IntegerType> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<FractionType> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => FractionType.Zero);
	}

	protected virtual IEnumerable<IntegerType> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<FractionType> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => FractionType.Zero);
	}

	protected virtual IEnumerable<IntegerType> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => IntegerType.Zero);
	}

	protected virtual IEnumerable<FractionType> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => FractionType.Zero);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Sum or
			DateTimeFunctionOperator.Sub => typeof(TimeSpanType),
			DateTimeFunctionOperator.Years or
			DateTimeFunctionOperator.Months or
			DateTimeFunctionOperator.Days or
			DateTimeFunctionOperator.Hours or
			DateTimeFunctionOperator.Minutes or
			DateTimeFunctionOperator.Seconds or
			DateTimeFunctionOperator.Milliseconds => typeof(IntegerType),
			DateTimeFunctionOperator.TotalDays or
			DateTimeFunctionOperator.TotalHours or
			DateTimeFunctionOperator.TotalMinutes or
			DateTimeFunctionOperator.TotalSeconds or
			DateTimeFunctionOperator.TotalMilliseconds => typeof(FractionType),


			_ => throw GetInvalidOperatorException()
		};
	}
}