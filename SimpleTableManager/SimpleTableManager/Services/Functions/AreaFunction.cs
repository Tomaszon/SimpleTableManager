﻿using System.Numerics;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class AreaFunction : FunctionBase<AreaFunctionOperator, Shape, decimal>
{
	public override IEnumerable<decimal> Execute()
	{
		//TODO produce multiple results for shapes with multiple different sides?
		return Operator switch
		{
			AreaFunctionOperator.Rectangle => Arguments.Select(p => p.Size1 * (p.Size2 ?? p.Size1)),

			AreaFunctionOperator.Ellipse =>
				Arguments.Select(p => p.Size1 * (p.Size2 ?? p.Size1) * Math.PI.ToType<decimal>()),

			AreaFunctionOperator.RightTriangle => Arguments.Select(p => p.Size1 * (p.Size2 ?? p.Size1) / 2),

			AreaFunctionOperator.IsoscaleTriangle => Arguments.Select(p => (p.Size2 ?? p.Size1) / 4 * Math.Sqrt((4 * p.Size1 * p.Size1 - ((p.Size2 * p.Size2) ?? (p.Size1 * p.Size1))).ToType<double>()).ToType<decimal>()),

			_ => throw GetInvalidOperatorException()
		};
	}
}