using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

//TODO make it work
public class StringFunction : Function<StringFunctionOperator>
{
	public override FunctionParameter Execute(IEnumerable<FunctionParameterArray> parameters = null)
	{
		return Operator switch
		{
			_ => throw new InvalidOperationException()
		};
	}

	protected override FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateParameters = null)
	{
		var hasSeparator = aggregateParameters.TryGetValue("separator", out var separator);

		var result = list.Aggregate(FunctionParameter.Default<string>(), (a, c) => a += hasSeparator ? AggregateCore(parameters, c) + new FunctionParameter(", ") : AggregateCore(parameters, c));

		(result.Value as string).TrimEnd(',', ' ');

		return result;
	}
}