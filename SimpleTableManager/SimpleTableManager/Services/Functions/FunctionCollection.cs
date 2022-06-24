using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
			{ typeof(NumericFunctionOperator), typeof(NumericFunction) },
			{ typeof(StringFunctionOperator), typeof(StringFunction) },
			{ typeof(ObjectFunctionOperator), typeof(ObjectFunction) }
		};
	}

	public static bool HasFunction(string functionOperatorName, [NotNullWhen(true)] out Type operatorType)
	{
		operatorType = _functions.Keys.SingleOrDefault(k => 
			Enum.GetNames(k).Contains(functionOperatorName, StringComparer.OrdinalIgnoreCase));

		return operatorType is not null;
	}

	public static IFunction GetFunction(string functionOperatorName, IEnumerable<IFunction> arguments)
	{
		HasFunction(functionOperatorName, out var key);

		var functionOperator = Enum.Parse(key, functionOperatorName, true);

		var type = _functions[key];

		return GetFunctionCore(type, functionOperator, arguments);
	}

	public static IFunction GetFunction(Enum functionOperator, IEnumerable<IFunction> arguments)
	{
		var type = _functions[functionOperator.GetType()];

		return GetFunctionCore(type, functionOperator, arguments);
	}

	public static IFunction GetFunctionCore(Type type, object functionOperator, IEnumerable<IFunction> arguments)
	{
		return (IFunction)Activator.CreateInstance(type, functionOperator, arguments.ToList());
	}
}
