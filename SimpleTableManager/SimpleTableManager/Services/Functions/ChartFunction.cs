namespace SimpleTableManager.Services.Functions;

public class ChartFunction : FunctionBase<ChartFunctionOperator, IConvertible, IChart>
{
	public override IEnumerable<IChart> ExecuteCore()
	{
		return Operator switch
		{
			ChartFunctionOperator.Raw => RawChart().Wrap(),
			ChartFunctionOperator.Scatter => ScatterChart().Wrap(),
			ChartFunctionOperator.Column => ColumnChart().Wrap(),
			ChartFunctionOperator.Bar => BarChart().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	public RawChart RawChart()
	{
		var xs = UnnamedArguments.Where(a => a.GroupingId == 0).SelectMany(a => a.Resolve());
		var ys = UnnamedArguments.Where(a => a.GroupingId == 1).SelectMany(a => a.Resolve());

		return new RawChart(xs, ys);
	}

	public IChart ScatterChart()
	{
		return null;
	}

	public IChart BarChart()
	{
		//HACK
		var xs = Arguments.First().Resolve();
		var ys = Arguments.Last().Resolve();

		return new BarChart(xs, ys);
	}

	public IChart ColumnChart()
	{
		return null;
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
			ChartFunctionOperator.Scatter => typeof(IChart),
			ChartFunctionOperator.Column => typeof(IChart),
			ChartFunctionOperator.Bar => typeof(BarChart),

			_ => throw GetInvalidOperatorException()
		};
	}
}
