namespace SimpleTableManager.Services.Functions;

public class ChartFunction<TDataX, TDataY> : FunctionBase<ChartFunctionOperator, TDataX, IChart>//HACK use TDataY somehow
	where TDataX : IParsable<TDataX>, IComparable, IConvertible
	where TDataY : IParsable<TDataY>, IComparable, IConvertible
{
	public override IEnumerable<IChart> ExecuteCore()
	{
		var xs = UnnamedArguments.Where(a => a.GroupingId is char x && x == 'X').SelectMany(a => a.Resolve().Cast<TDataX>());
		var ys = UnnamedArguments.Where(a => a.GroupingId is char y && y == 'Y').SelectMany(a => a.Resolve().Cast<TDataY>());

		return Operator switch
		{
			ChartFunctionOperator.Raw => new RawChart<TDataX, TDataY>(xs, ys).Wrap(),
			// ChartFunctionOperator.Scatter => .Wrap(),
			// ChartFunctionOperator.Column => .Wrap(),
			ChartFunctionOperator.Bar => new BarChart<TDataX, TDataY>(xs, ys).Wrap(),

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
			ChartFunctionOperator.Raw => typeof(RawChart<TDataX, TDataY>),
			// ChartFunctionOperator.Scatter => typeof(IChart),
			// ChartFunctionOperator.Column => typeof(IChart),
			ChartFunctionOperator.Bar => typeof(BarChart<TDataX, TDataY>),

			_ => throw GetInvalidOperatorException()
		};
	}
}
