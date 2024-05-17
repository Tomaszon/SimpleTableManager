using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class AreaFunction : FunctionBase<AreaFunctionOperator, Shape, decimal>
{
	public override IEnumerable<decimal> Execute()
	{
		//TODO produce multiple results for shapes with multiple different sides?
		return Operator switch
		{
			AreaFunctionOperator.Square => Arguments.Select(p => p.Size1 * p.Size1),

			// AreaFunctionOperator.Rectangle => (Arguments.First() * Arguments.Last()).Wrap(),

			AreaFunctionOperator.Circle => Arguments.Select(p => p.Size1 * p.Size1 * Math.PI.ToType<decimal>()),

			// AreaFunctionOperator.Ellipse =>
			// 	(Arguments.First() * Arguments.Last() * Math.PI.ToType<decimal>()).Wrap(),

			// AreaFunctionOperator.RightTriangle => (Arguments.First() * Arguments.Last() / 2).Wrap(),

			// AreaFunctionOperator.IsoscaleRightTriangle => Arguments.Select(x => x * x / 2),

			_ => throw GetInvalidOperatorException()
		};
	}
}
