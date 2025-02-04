namespace SimpleTableManager.Services.Functions;

public class ChartFunction : FunctionBase<ChartFunctionOperator, IConvertible, Chart>
{
	public override IEnumerable<Chart> ExecuteCore()
	{
		return Operator switch
		{
			ChartFunctionOperator.Const =>
				new Chart(Arguments.First().Resolve(), Arguments.Last().Resolve()).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	public override string GetFriendlyName()
	{
		return nameof(Chart);
	}
}