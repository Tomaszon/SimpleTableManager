namespace SimpleTableManager.Services.Functions;

public static class FunctionCollection
{
	public static List<Type> Functions { get; set; }

	static FunctionCollection()
	{
		Functions = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
			t.Namespace == typeof(IFunction).Namespace && !t.IsAbstract && !t.IsInterface && !t.IsGenericType && !t.IsNested).ToList();
	}

	public static IFunction GetFunction(string typeName, string functionOperator, Dictionary<ArgumentName, string>? namedArguments, IEnumerable<object> arguments)
	{
		var bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

		var functionType = Functions.Single(f => GetRootClass(f).GenericTypeArguments[1] == ContentParser.GetTypeByFriendlyName(typeName));

		var argumentsProperty = functionType.GetProperty(nameof(IFunction.Arguments), bindingFlags)!;

		var argsInnerType = argumentsProperty.PropertyType.GenericTypeArguments.First();

		var targetArray = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(argsInnerType))!;

		var parsedArgs = argsInnerType == typeof(string) ? arguments! : arguments.Select(a => a is string str ? ContentParser.ParseStringValue(argsInnerType, str!) : a);

		parsedArgs.ForEach(e => targetArray.Add(e));

		var instance = (IFunction)Activator.CreateInstance(functionType)!;

		var operatorPoperty = functionType.GetProperty(nameof(IFunction.Operator), bindingFlags)!;

		if(!Enum.TryParse(operatorPoperty.PropertyType, functionOperator, true, out var op))
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
}