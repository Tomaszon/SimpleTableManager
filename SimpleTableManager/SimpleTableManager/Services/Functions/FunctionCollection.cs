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

	public static IFunction GetFunction(Type valueType, string functionOperator, IEnumerable<IFunctionArgument> arguments)
	{
		var functionType = Functions[valueType];

		var instance = (IFunction)Activator.CreateInstance(functionType)!;

		var bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

		var operatorProperty = functionType.GetProperty(nameof(IFunction.Operator), bindingFlags)!;

		var op = Enum.Parse(operatorProperty.PropertyType, functionOperator,
		true);

		operatorProperty.SetValue(instance, op);

		if (arguments is not null)
		{
			instance.Arguments = [.. arguments];
		}

		return instance;
	}

	public static IFunction GetFunction<T>(Enum functionOperator, IEnumerable<IFunctionArgument> arguments)
	{
		return GetFunction(typeof(T), functionOperator.ToString(), arguments);
	}

	private static Type GetRootClass(Type type)
	{
		return type.BaseType is not null && type.BaseType != typeof(object) ? GetRootClass(type.BaseType) : type;
	}
}