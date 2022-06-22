using System;
using System.Collections.Generic;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class ObjectFunction : Function<ObjectFunctionOperator, object>
{
	public ObjectFunction(ObjectFunctionOperator functionOperator, List<FunctionParameter> arguments) : base(functionOperator, arguments)
	{

	}

	public override List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null)
	{
		return Arguments;
	}

	protected override FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		throw new NotSupportedException();
	}
}