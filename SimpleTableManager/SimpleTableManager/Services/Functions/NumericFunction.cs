using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class NumericFunction : Function
{
	public NumericFunctionType Type { get; set; }

	public NumericFunction(NumericFunctionType type, params FunctionParameter[] arguments) : base(arguments)
	{
		Type = type;
	}

	public override FunctionParameter Execute(IEnumerable<FunctionParameter> parameters = null)
	{
		return Type switch
		{
			NumericFunctionType.Sum => Sum(parameters),
			NumericFunctionType.Avg => Avg(parameters),

			_ => throw new InvalidOperationException()
		};
	}

	private FunctionParameter Sum(IEnumerable<FunctionParameter> parameters)
	{
		var result = Arguments.Aggregate(FunctionParameter.Default<decimal>(), (a, v) => a += (new Func<FunctionParameter>(() =>
		{
			if (parameters is not null && parameters.SingleOrDefault(p => p.ReferredCellPosition == v.ReferredCellPosition) is var x && !x.Equals(default))
			{
				return new FunctionParameter(x.Value);
			}
			else
			{
				return v;
			}
		})).Invoke());

		return result;
	}

	private FunctionParameter Avg(IEnumerable<FunctionParameter> parameters)
	{
		var result = Sum(parameters) / new FunctionParameter(Arguments.Count);

		return result;
	}
}

public abstract class Function
{
	public abstract FunctionParameter Execute(IEnumerable<FunctionParameter> parameters = null);

	public List<Position> GetReferredCellPositions() => Arguments.Where(a => a.ReferredCellPosition is not null).Select(a => a.ReferredCellPosition).ToList();

	public List<FunctionParameter> Arguments { get; init; }

	protected Function(params FunctionParameter[] arguments)
	{
		Arguments = arguments.ToList();
	}
}