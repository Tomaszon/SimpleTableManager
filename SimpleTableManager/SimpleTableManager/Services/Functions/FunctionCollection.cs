using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions;

public static class FunctionCollection
{
	private static Dictionary<Type, Type> _functions;

	static FunctionCollection()
	{
		_functions = new Dictionary<Type, Type>()
		{
			{ typeof(NumericFunctionType), typeof(NumericFunction<>) }
		};
	}

	public static Function<TReturn> GetFunction<TReturn>(string functionTypeName, params TReturn[] arguments)
	{
		var key = _functions.Keys.Single(k => Enum.TryParse(k, functionTypeName, true, out _));

		var functionType = Enum.Parse(key, functionTypeName, true);

		var type = _functions[key];

		return GetFunctionCore<TReturn>(type, functionType, arguments);
	}

	public static Function<TReturn> GetFunction<TFunctionType, TReturn>(TFunctionType functionType, params TReturn[] arguments)
	{
		var type = _functions[typeof(TFunctionType)];

		return GetFunctionCore<TReturn>(type, functionType, arguments);
	}

	private static Function<TReturn> GetFunctionCore<TReturn>(Type type, object functionType, TReturn[] arguments)
	{
		var genericType = type.MakeGenericType(typeof(TReturn));

		return (Function<TReturn>)Activator.CreateInstance(genericType, new object[] { functionType, arguments });
	}
}
