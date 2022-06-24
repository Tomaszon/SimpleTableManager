using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class ObjectFunction : Function<ObjectFunctionOperator, object>
{
	public static ObjectFunction Empty() => new ObjectFunction(ObjectFunctionOperator.Const, new List<IFunction>());

	public ObjectFunction(ObjectFunctionOperator functionOperator, List<IFunction> arguments) : base(functionOperator, arguments)
	{

	}

	public override List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null)
	{
		return Arguments.SelectMany(a => a.Execute(parameters)).ToList();
	}

	protected override FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		throw new NotSupportedException();
	}
}