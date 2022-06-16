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
			{ typeof(NumericFunctionType), typeof(NumericFunction) }
		};
	}

	public static Function GetFunction(string functionTypeName, IEnumerable<FunctionParameter> arguments)
	{
		var key = _functions.Keys.Single(k => Enum.TryParse(k, functionTypeName, true, out _));

		var functionType = Enum.Parse(key, functionTypeName, true);

		var type = _functions[key];

		return GetFunctionCore(type, functionType, arguments);
	}

	public static Function GetFunction(Enum functionType, IEnumerable<FunctionParameter> arguments)
	{
		var type = _functions[functionType.GetType()];

		return GetFunctionCore(type, functionType, arguments);
	}

	private static Function GetFunctionCore(Type type, object functionType, IEnumerable<FunctionParameter> arguments)
	{
		return (Function)Activator.CreateInstance(type, functionType, arguments.ToList());
	}
}
