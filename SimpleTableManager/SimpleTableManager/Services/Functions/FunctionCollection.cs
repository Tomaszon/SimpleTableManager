using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public static class FunctionCollection
{
	private static List<(Type, Type)> _functions;

	static FunctionCollection()
	{
		_functions = new List<(Type, Type)>()
		{
			(typeof(NumericFunctionType), typeof(NumericFunction) )
		};
	}

	public static Function GetFunction(string functionTypeName, params FunctionParameter[] arguments)
	{
		var key = _functions.Select(e => e.Item1).Single(k => Enum.TryParse(k, functionTypeName, true, out _));

		var functionType = Enum.Parse(key, functionTypeName, true);

		var type = _functions.Single(e => e.Item1 == key).Item2;

		return GetFunctionCore(type, functionType, arguments);
	}

	public static Function GetFunction(Enum functionType, params FunctionParameter[] arguments)
	{
		var type = _functions.Single(e => e.Item1 == functionType.GetType()).Item2;

		return GetFunctionCore(type, functionType, arguments);
	}

	private static Function GetFunctionCore(Type type, object functionType, FunctionParameter[] arguments)
	{
		return (Function)Activator.CreateInstance(type, functionType, arguments);
	}
}
