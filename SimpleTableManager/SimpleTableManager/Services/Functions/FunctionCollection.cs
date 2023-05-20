using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;

namespace SimpleTableManager.Services.Functions
{
	public static class FunctionCollection
	{
		public static Dictionary<Type, (Type, Type)> Functions { get; set; }

		static FunctionCollection()
		{
			Functions = new Dictionary<Type, (Type, Type)>
			{
				{ typeof(int), (typeof(IntegerNumericFunction), typeof(NumericFunctionOperator)) },
				{ typeof(decimal), (typeof(DecimalNumericFunction), typeof(NumericFunctionOperator)) },
				{ typeof(string), (typeof(StringFunction), typeof(StringFunctionOperator)) },
				{ typeof(bool), (typeof(BooleanFunction), typeof(BooleanFunctionOperator)) }
			};
		}

		public static IFunction GetFunction(string typeName, string functionOperator, Dictionary<string, string> namedArguments, IEnumerable<object> arguments)
		{
			var types = Functions[Shared.GetTypeByName(typeName)];
			var op = Enum.Parse(types.Item2, functionOperator, true);
			var args = arguments?.Select(a => a.ToString());

			var argsInnerType = types.Item1.GetProperty(nameof(FunctionBase<Enum, object>.Arguments)).PropertyType.GenericTypeArguments.First();

			var targetArray = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(argsInnerType));

			var parsedArgs = argsInnerType == typeof(string) ? args : args.Select(a => Shared.ParseStringValue(argsInnerType, a));

			parsedArgs.ForEach(e => targetArray.Add(e));

			var instance = (IFunction)Activator.CreateInstance(types.Item1);

			types.Item1.GetProperty(nameof(FunctionBase<Enum, object>.Operator)).SetValue(instance, op);
			if (namedArguments is not null)
			{
				types.Item1.GetProperty(nameof(FunctionBase<Enum, object>.NamedArguments)).SetValue(instance, namedArguments);
			}
			if (arguments is not null)
			{
				types.Item1.GetProperty(nameof(FunctionBase<Enum, object>.Arguments)).SetValue(instance, targetArray);
			}

			return instance;
		}
	}
}