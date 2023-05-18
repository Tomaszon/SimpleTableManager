using System;
using System.Collections.Generic;

namespace SimpleTableManager.Services.Functions
{
	public static class FunctionCollection2
	{
		public static Dictionary<Type, (Type, Type)> Functions { get; set; }

		static FunctionCollection2()
		{
			Functions = new Dictionary<Type, (Type, Type)>
			{
				{ typeof(int), (typeof(IntegerNumericFunction2), typeof(NumericFunctionOperator)) },
				{ typeof(decimal), (typeof(DecimalNumericFunction2), typeof(NumericFunctionOperator)) },
				{ typeof(string), (typeof(StringFunction2), typeof(StringFunctionOperator)) }
			};
		}

		public static IFunction2 GetFunction(string typeName, string functionOperator, Dictionary<string, object> namedArguments, IEnumerable<object> arguments)
		{
			var types = Functions[Shared.GetTypeByName(typeName)];
			var op = Enum.Parse(types.Item2, functionOperator, true);

			return (IFunction2)Activator.CreateInstance(types.Item1, op, namedArguments, arguments);
		}
	}
}