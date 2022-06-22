using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class NumericFunction : Function<NumericFunctionOperator, decimal>
{
	public NumericFunction(NumericFunctionOperator functionOperator, List<FunctionParameter> arguments) :
		base(functionOperator, arguments)
	{

	}

	public override List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null)
	{
		return new List<FunctionParameter>()
		{
			Operator switch
			{
				NumericFunctionOperator.Sum => Sum(parameters),
				NumericFunctionOperator.Avg => Avg(parameters),

				_ => throw new InvalidOperationException()
			}
		};
	}

	private FunctionParameter Sum(IEnumerable<FunctionParameterArray> parameters)
	{
		return Aggregate(Arguments, parameters);
	}

	protected override FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		return list.Aggregate(FunctionParameter.Default<decimal>(), (a, c) => a += AggregateCore(parameters, c));
	}

	private FunctionParameter Avg(IEnumerable<FunctionParameterArray> parameters)
	{
		return Sum(parameters) / new FunctionParameter(Arguments.Count());
	}
}
