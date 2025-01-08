using System.Data;

namespace SimpleTableManager.Services.Functions;

public static class FunctionCollection
{
	public static Dictionary<Type, Type> Functions { get; set; } = [];

	static FunctionCollection()
	{
		var functions = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
			t.Namespace == typeof(IFunction).Namespace && !t.IsAbstract &&
			!t.IsInterface && !t.IsGenericType && !t.IsNested);

		Functions = functions.SelectMany(f =>
			f.GetCustomAttributes<FunctionMappingTypeAttribute>().Select(a =>
				(a.MappingType, FunctionType: f))).ToDictionary(k => k.MappingType, v => v.FunctionType);
	}

	public static IFunction GetFunction<T>(string functionOperator, Dictionary<ArgumentName, IFunctionArgument>? namedArguments, IEnumerable<IFunctionArgument> arguments)
	{
		return GetFunction(typeof(T), functionOperator, namedArguments, arguments);
	}

	public static IFunction GetFunction(Type argType, string functionOperator, Dictionary<ArgumentName, IFunctionArgument>? namedArguments, IEnumerable<IFunctionArgument> arguments)
	{
		var functionType = Functions[argType];

		var bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

		var instance = (IFunction)Activator.CreateInstance(functionType)!;

		var operatorProperty = functionType.GetProperty(nameof(IFunction.Operator), bindingFlags)!;

		if (!Enum.TryParse(operatorProperty.PropertyType, functionOperator, true, out var op))
		{
			throw new ArgumentException($"Operator '{functionOperator}' is not valid! Values={string.Join('|', Enum.GetNames(operatorProperty.PropertyType))}");
		}

		operatorProperty.SetValue(instance, op);

		if (namedArguments is not null)
		{
			var namedArgumentsProperty = functionType.GetProperty(nameof(IFunction.NamedArguments))!;
			
			namedArgumentsProperty.SetValue(instance, namedArguments);
		}
		if (arguments is not null)
		{
			var argumentsProperty = functionType.GetProperty(nameof(IFunction.Arguments), bindingFlags)!;

			argumentsProperty.SetValue(instance, arguments);
		}

		return instance;
	}

	private static Type GetRootClass(Type type)
	{
		return type.BaseType is not null && type.BaseType != typeof(object) ? GetRootClass(type.BaseType) : type;
	}
}