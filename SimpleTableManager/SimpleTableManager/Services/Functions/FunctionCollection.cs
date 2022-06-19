using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public static class FunctionCollection
{
	private static Dictionary<Type, Type> _functions;

	static FunctionCollection()
	{
		_functions = new Dictionary<Type, Type>()
		{
			{ typeof(NumericFunctionOperator), typeof(NumericFunction) }
		};
	}

	public static IFunction GetFunction(string functionOperatorName, IEnumerable<FunctionParameter> arguments)
	{
		var key = _functions.Keys.Single(k => Enum.TryParse(k, functionOperatorName, true, out _));

		var functionOperator = Enum.Parse(key, functionOperatorName, true);

		var type = _functions[key];

		return GetFunctionCore(type, functionOperator, arguments);
	}

	public static IFunction GetFunction(Enum functionOperator, IEnumerable<FunctionParameter> arguments)
	{
		var type = _functions[functionOperator.GetType()];

		return GetFunctionCore(type, functionOperator, arguments);
	}

	private static IFunction GetFunctionCore(Type type, object functionOperator, IEnumerable<FunctionParameter> arguments)
	{
		return (IFunction)Activator.CreateInstance(type, functionOperator, arguments.ToList());
	}
}
