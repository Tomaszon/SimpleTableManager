using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

public abstract class DateTimeFunctionBase<TIn> :
	FunctionBase<DateTimeFunctionOperator, TIn, object>
	where TIn : IParsable<TIn>, IConvertible, IComparable
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => UnwrappedUnnamedArguments.Cast<object>(),
			DateTimeFunctionOperator.Avg => Avg().Wrap(),
			DateTimeFunctionOperator.Years => Years().Cast<object>(),
			DateTimeFunctionOperator.Months => Months().Cast<object>(),
			DateTimeFunctionOperator.Days => Days().Cast<object>(),
			DateTimeFunctionOperator.Hours => Hours().Cast<object>(),
			DateTimeFunctionOperator.Minutes => Minutes().Cast<object>(),
			DateTimeFunctionOperator.Seconds => Seconds().Cast<object>(),
			DateTimeFunctionOperator.Milliseconds => Milliseconds().Cast<object>(),
			DateTimeFunctionOperator.TotalDays => TotalDays().Cast<object>(),
			DateTimeFunctionOperator.TotalHours => TotalHours().Cast<object>(),
			DateTimeFunctionOperator.TotalMinutes => TotalMinutes().Cast<object>(),
			DateTimeFunctionOperator.TotalSeconds => TotalSeconds().Cast<object>(),
			DateTimeFunctionOperator.TotalMilliseconds => TotalMilliseconds().Cast<object>(),
			DateTimeFunctionOperator.Greater => Greater().Wrap(),
			DateTimeFunctionOperator.Less => Less().Wrap(),
			DateTimeFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap(),
			DateTimeFunctionOperator.LessOrEquals => LessOrEquals().Wrap(),
			DateTimeFunctionOperator.Equals => Equals().Wrap(),
			DateTimeFunctionOperator.NotEquals => NotEquals().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected abstract object Avg();

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
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Avg or
			DateTimeFunctionOperator.Min or
			DateTimeFunctionOperator.Max or
			DateTimeFunctionOperator.Now or
			DateTimeFunctionOperator.Tomorrow or
			DateTimeFunctionOperator.Yesterday => typeof(TIn),
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