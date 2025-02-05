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
		//HACK
		var xs = Arguments.First().Resolve();
		var ys = Arguments.Last().Resolve();

		return new RawChart(xs, ys);
	}

	public IChart ScatterChart()
	{
		return null;
	}

	public IChart BarChart()
	{
		return null;
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
			ChartFunctionOperator.Bar => typeof(IChart),

			_ => throw GetInvalidOperatorException()
		};
	}
}
