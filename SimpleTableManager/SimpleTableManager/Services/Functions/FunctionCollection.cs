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

	public static IFunction GetFunction<T>(Enum functionOperator, IEnumerable<IFunctionArgument> arguments)
	{
		var functionType = Functions[typeof(T)];

		var instance = (IFunction)Activator.CreateInstance(functionType)!;

		instance.Operator = functionOperator;

		if (arguments is not null)
		{
			instance.Arguments = arguments;
		}

		return instance;
	}

	private static Type GetRootClass(Type type)
	{
		return type.BaseType is not null && type.BaseType != typeof(object) ? GetRootClass(type.BaseType) : type;
	}
}