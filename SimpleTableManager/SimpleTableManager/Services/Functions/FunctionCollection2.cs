using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Extensions;

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
				{ typeof(string), (typeof(StringFunction2), typeof(StringFunctionOperator)) },
				{ typeof(bool), (typeof(BooleanFunction2), typeof(BooleanFunctionOperator)) }
			};
		}

		public static IFunction2 GetFunction(string typeName, string functionOperator, Dictionary<string, string> namedArguments, IEnumerable<object> arguments)
		{
			var types = Functions[Shared.GetTypeByName(typeName)];
			var op = Enum.Parse(types.Item2, functionOperator, true);
			var args = arguments.Select(a => a.ToString());

			//TODO maybe get this by a custom attribute?
			var constructor = types.Item1.GetConstructors().Where(c => c.GetParameters().Count() != 0).First();

			var argsInnerType = constructor.GetParameters().Last().ParameterType.GenericTypeArguments.First();

			var targetArray = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(argsInnerType));

			var parsedArgs = argsInnerType == typeof(string) ? args : args.Select(a => Shared.ParseStringValue(argsInnerType, a));

			parsedArgs.ForEach(e => targetArray.Add(e));

			return (IFunction2)constructor.Invoke(new object[] { op, namedArguments, targetArray });
		}
	}
}