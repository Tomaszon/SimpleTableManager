using System.Data;

namespace SimpleTableManager.Services.Functions;

public static class FunctionCollection
{
	public static Dictionary<Type, Type> Functions { get; set; } = new Dictionary<Type, Type>();

	static FunctionCollection()
	{
		var functions = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
			t.Namespace == typeof(IFunction).Namespace && !t.IsAbstract && 
			!t.IsInterface && !t.IsGenericType && !t.IsNested);

		Functions = functions.SelectMany(f => 
			f.GetCustomAttributes<FunctionMappingTypeAttribute>().Select(a => 
				(a.MappingType, FunctionType: f))).ToDictionary(k => k.MappingType, v => v.FunctionType);
	}

	public static IFunction GetFunction<T>(string functionOperator, Dictionary<ArgumentName, string>? namedArguments, IEnumerable<object> arguments)
	{
		return GetFunction(typeof(T), functionOperator, namedArguments, arguments);
	}

	public static IFunction GetFunction(string typeName, string functionOperator, Dictionary<ArgumentName, string>? namedArguments, IEnumerable<object> arguments)
	{
		return GetFunction(ContentParser.GetTypeByFriendlyName(typeName), functionOperator, namedArguments, arguments);
	}

	public static IFunction GetFunction(Type argType, string functionOperator, Dictionary<ArgumentName, string>? namedArguments, IEnumerable<object> arguments)
	{
		var functionType = Functions[argType];

		var bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

		var argumentsProperty = functionType.GetProperty(nameof(IFunction.Arguments), bindingFlags)!;

		var targetArray = ParseArgumentList(arguments, argType);

		var instance = (IFunction)Activator.CreateInstance(functionType)!;

		var operatorPoperty = functionType.GetProperty(nameof(IFunction.Operator), bindingFlags)!;

		if (!Enum.TryParse(operatorPoperty.PropertyType, functionOperator, true, out var op))
		{
			throw new ArgumentException($"Operator '{functionOperator}' is not valid! Values={string.Join('|', Enum.GetNames(operatorPoperty.PropertyType))}");
		}

		operatorPoperty.SetValue(instance, op);

		if (namedArguments is not null)
		{
			functionType.GetProperty(nameof(IFunction.NamedArguments))!.SetValue(instance, namedArguments);
		}
		if (arguments is not null)
		{
			argumentsProperty.SetValue(instance, targetArray);
		}

		return instance;
	}

	private static Type GetRootClass(Type type)
	{
		return type.BaseType is not null && type.BaseType != typeof(object) ? GetRootClass(type.BaseType) : type;
	}

	public static System.Collections.IList ParseArgumentList(IEnumerable<object> arguments, Type argType)
	{
		var targetArray = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(argType))!;

		var parsedArgs = argType == typeof(string) ? arguments! : arguments.Select(a => a is string str ? ContentParser.ParseStringValue(argType, str!) : a);

		parsedArgs.ForEach(e => targetArray.Add(e));

		return targetArray;
	}
}