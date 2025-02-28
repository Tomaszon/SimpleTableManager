namespace SimpleTableManager.Services.Functions;

public class ChartFunction : FunctionBase<ChartFunctionOperator, double, IChart>//HACK double
{
	public override IEnumerable<IChart> ExecuteCore()
	{
		var xs = UnnamedArguments.Where(a => a.GroupingId is char x && x == 'X').SelectMany(a => a.Resolve());
		var ys = UnnamedArguments.Where(a => a.GroupingId is char y && y == 'Y').SelectMany(a => a.Resolve());

		return Operator switch
		{
			ChartFunctionOperator.Raw => new RawChart(xs, ys).Wrap(),
			// ChartFunctionOperator.Scatter => .Wrap(),
			// ChartFunctionOperator.Column => .Wrap(),
			ChartFunctionOperator.Bar => new BarChart(xs, ys).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	public override string GetFriendlyName()
	{
		return "Chart";
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			ChartFunctionOperator.Raw => typeof(RawChart),
			// ChartFunctionOperator.Scatter => typeof(IChart),
			// ChartFunctionOperator.Column => typeof(IChart),
			ChartFunctionOperator.Bar => typeof(BarChart),

			_ => throw GetInvalidOperatorException()
		};
	}
}
