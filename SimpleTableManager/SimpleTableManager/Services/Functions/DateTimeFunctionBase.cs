namespace SimpleTableManager.Services.Functions;

public abstract class DateTimeFunctionBase<TIn, TOut> :
	FunctionBase<DateTimeFunctionOperator, TIn, TOut>
	where TIn : IConvertible
	where TOut : IConvertible
{
	protected const int MONTHS_IN_A_YEAR = 12;
	protected const double DAYS_IN_A_MONTH = 30.42;
	protected const int DAYS_IN_A_YEAR = 365;

	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Years => Years().Cast<TOut>(),
			DateTimeFunctionOperator.Months => Months().Cast<TOut>(),
			DateTimeFunctionOperator.Days => Days().Cast<TOut>(),
			DateTimeFunctionOperator.Hours => Hours().Cast<TOut>(),
			DateTimeFunctionOperator.Minutes => Minutes().Cast<TOut>(),
			DateTimeFunctionOperator.Seconds => Seconds().Cast<TOut>(),
			DateTimeFunctionOperator.Milliseconds => Milliseconds().Cast<TOut>(),
			DateTimeFunctionOperator.TotalYears => TotalYears().Cast<TOut>(),
			DateTimeFunctionOperator.TotalMonths => TotalMonths().Cast<TOut>(),
			DateTimeFunctionOperator.TotalDays => TotalDays().Cast<TOut>(),
			DateTimeFunctionOperator.TotalHours => TotalHours().Cast<TOut>(),
			DateTimeFunctionOperator.TotalMinutes => TotalMinutes().Cast<TOut>(),
			DateTimeFunctionOperator.TotalSeconds => TotalSeconds().Cast<TOut>(),
			DateTimeFunctionOperator.TotalMilliseconds => TotalMilliseconds().Cast<TOut>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected virtual IEnumerable<int> Years()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalYears()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

	protected virtual IEnumerable<int> Months()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalMonths()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

	protected virtual IEnumerable<int> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalDays()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

	protected virtual IEnumerable<int> Hours()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalHours()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

	protected virtual IEnumerable<int> Minutes()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalMinutes()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

	protected virtual IEnumerable<int> Seconds()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalSeconds()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

	protected virtual IEnumerable<int> Milliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<double> TotalMilliseconds()
	{
		return UnwrappedUnnamedArguments.Select(a => 0d);
	}

    public override Type GetOutType()
    {
        return Operator switch
		{
			DateTimeFunctionOperator.Sum or
			DateTimeFunctionOperator.Sub => typeof(ConvertibleTimeSpan),
			DateTimeFunctionOperator.Years or
			DateTimeFunctionOperator.Months or
			DateTimeFunctionOperator.Days or
			DateTimeFunctionOperator.Hours or
			DateTimeFunctionOperator.Minutes or
			DateTimeFunctionOperator.Seconds or
			DateTimeFunctionOperator.Milliseconds => typeof(int),
			DateTimeFunctionOperator.TotalYears or
			DateTimeFunctionOperator.TotalMonths or
			DateTimeFunctionOperator.TotalDays or
			DateTimeFunctionOperator.TotalHours or
			DateTimeFunctionOperator.TotalMinutes or
			DateTimeFunctionOperator.TotalSeconds or
			DateTimeFunctionOperator.TotalMilliseconds => typeof(double),


			_ => throw GetInvalidOperatorException()
		};
    }
}