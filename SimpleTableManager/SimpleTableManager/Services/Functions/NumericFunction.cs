using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class NumericFunction : Function<NumericFunctionOperator, decimal>
{
	public NumericFunction(NumericFunctionOperator functionOperator, List<IFunction> arguments) :
		base(functionOperator, arguments)
	{

	}

	public override List<ObjectFunction> Execute(IEnumerable<GroupedObjectFunctions> parameters = null)
	{
		return new List<ObjectFunction>()
		{
			Operator switch
			{
				NumericFunctionOperator.Sum => Sum(parameters),
				NumericFunctionOperator.Avg => Avg(parameters),

				_ => throw new InvalidOperationException()
			}
		};
	}

	private ObjectFunction Sum(IEnumerable<GroupedObjectFunctions> parameters)
	{
		return Aggregate(Arguments.SelectMany(a => a.Execute(parameters)), parameters);
	}

	protected override ObjectFunction Aggregate(IEnumerable<ObjectFunction> list, IEnumerable<GroupedObjectFunctions> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		return list.Aggregate(ObjectFunction.Default<decimal>(), (a, c) => a += AggregateCore(parameters, c));
	}

	private ObjectFunction Avg(IEnumerable<GroupedObjectFunctions> parameters)
	{
		return new ObjectFunction((Sum(parameters) / new ObjectFunction(Arguments.Count())).Value);
	}
}
