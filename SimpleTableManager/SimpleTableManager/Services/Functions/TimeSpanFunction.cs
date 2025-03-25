using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<double>(ArgumentName.Multiplier, 1)]
[NamedArgument<double>(ArgumentName.Divider, 1)]
[FunctionMappingType(typeof(ConvertibleTimeSpan))]
public class TimeSpanFunction : DateTimeFunctionBase<ConvertibleTimeSpan>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const => UnwrappedUnnamedArguments,
			DateTimeFunctionOperator.Sum => Sum().Wrap(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),
			DateTimeFunctionOperator.Mul => Mul(),
			DateTimeFunctionOperator.Div => Div(),
			DateTimeFunctionOperator.Min => Min(TimeSpan.MinValue).Wrap(),
			DateTimeFunctionOperator.Max => Max(TimeSpan.MaxValue).Wrap(),

			_ => base.ExecuteCore()
		};
	}

	private ConvertibleTimeSpan Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TimeSpan.Zero, (a, c) => a + c);
	}

	private ConvertibleTimeSpan Sub()
	{
		return UnwrappedUnnamedArgumentsIfAny(TimeSpan.Zero, () => UnwrappedUnnamedArguments.Skip(1).Aggregate((TimeSpan)UnwrappedUnnamedArguments.First(), (a, c) => a - c));
	}

	private IEnumerable<ConvertibleTimeSpan> Mul()
	{
		var multiplier = GetNamedArgument<double>(ArgumentName.Multiplier);

		return UnwrappedUnnamedArguments.Select(a => a.Multiply(multiplier));
	}

	private IEnumerable<ConvertibleTimeSpan> Div()
	{
		var divider = GetNamedArgument<double>(ArgumentName.Divider);

		return UnwrappedUnnamedArguments.Select(a => a.Divide(divider));
	}

	protected override ConvertibleTimeSpan Avg()
	{
		return UnwrappedUnnamedArgumentsIfAny<TimeSpan>(TimeSpan.Zero, () => Sum().Divide(UnwrappedUnnamedArguments.Count()));
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
			DateTimeFunctionOperator.Mul or
			DateTimeFunctionOperator.Div => typeof(ConvertibleTimeSpan),

			_ => base.GetOutType()
		};
	}
}