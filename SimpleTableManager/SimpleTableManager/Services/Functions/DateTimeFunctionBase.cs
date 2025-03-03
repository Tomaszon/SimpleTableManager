using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

public abstract class DateTimeFunctionBase<TIn, TOut> :
	FunctionBase<DateTimeFunctionOperator, TIn, TOut>
	where TIn : IParsable<TIn>, IConvertible, IComparable
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
			DateTimeFunctionOperator.Greater => Greater().Wrap().Cast<TOut>(),
			DateTimeFunctionOperator.Less => Less().Wrap().Cast<TOut>(),
			DateTimeFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap().Cast<TOut>(),
			DateTimeFunctionOperator.LessOrEquals => LessOrEquals().Wrap().Cast<TOut>(),
			DateTimeFunctionOperator.Equals => Equals().Wrap().Cast<TOut>(),
			DateTimeFunctionOperator.NotEquals => NotEquals().Wrap().Cast<TOut>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected abstract TOut Avg();

	protected virtual IEnumerable<int> Years()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
	}

	protected virtual IEnumerable<int> Months()
	{
		return UnwrappedUnnamedArguments.Select(a => 0);
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
			>= DateTimeFunctionOperator.Years and
			<= DateTimeFunctionOperator.Milliseconds => typeof(int),
			>= DateTimeFunctionOperator.TotalDays and
			<= DateTimeFunctionOperator.TotalMilliseconds => typeof(double),
			>= DateTimeFunctionOperator.Greater and
			<= DateTimeFunctionOperator.NotEquals => typeof(FormattableBoolean),

			_ => throw GetInvalidOperatorException()
		};
	}
}