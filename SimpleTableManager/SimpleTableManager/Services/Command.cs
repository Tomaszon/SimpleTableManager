using System.Dynamic;

namespace SimpleTableManager.Services;

public class Command
{
	public CommandReference? Reference { get; set; }

	public List<string>? Arguments { get; set; }

	public List<string>? AvailableKeys { get; set; }

	public string RawCommand { get; set; }

	public Command(CommandReference? reference, string rawCommand, List<string>? arguments)
	{
		Reference = reference;
		RawCommand = rawCommand;
		Arguments = arguments;
	}

	public static Command FromString(string rawCommand)
	{
		var reference = GetCommandReference(rawCommand, out var arguments);

		return new Command(reference, rawCommand, arguments);
	}

	public List<object?> Execute(IEnumerable<IStateModifierCommandExecuter> instances, Type type)
	{
		List<object?> results = new();

		var method = GetMethod(type);
		var parameters = GetParameters(method);

		if (parameters.Count(p => !p.IsOptional) > Arguments?.Count ||
			parameters.All(p => !p.IsArray) && parameters.Count < Arguments?.Count)
		{
			throw new ArgumentCountException(RawCommand, Reference);
		}

		List<object?> parsedArguments = new();
		List<string> validationResults = new();

		for (var i = 0; i < parameters.Count; i++)
		{
			var paramType = parameters[i].Type;

			if (paramType.IsArray)
			{
				var values = ParseArrayValues(parameters, i, paramType);

				validationResults.AddRange(ValidateCollectionArgument(values, parameters[i]));

				if (values is not null)
				{
					foreach (var e in values)
					{
						validationResults.AddRange(ValidateArgumentElement(e, parameters[i]));
					}
				}

				parsedArguments.Add(values);
			}
			else
			{
				var value = i < Arguments?.Count ?
					ContentParser.ParseStringValue(paramType, Arguments[i]) : parameters[i].DefaultValue;

				validationResults.AddRange(ValidateArgumentElement(value, parameters[i]));

				parsedArguments.Add(value);
			}
		}

		if (validationResults.Count > 0)
		{
			throw new ArgumentException(string.Join('\n', validationResults));
		}

		try
		{
			instances.ForEach(i =>
			{
				var attribute = method.GetCustomAttribute<CommandFunctionAttribute>()!;

				var endReferencedObject = attribute.IgnoreReferencedObject ? i : i.GetEndReferencedObject();

				results.Add(method.Invoke(endReferencedObject, [.. parsedArguments]));

				if (attribute!.StateModifier)
				{
					endReferencedObject.InvokeStateModifierCommandExecutedEvent(new StateModifierCommandExecutedEventArgs(endReferencedObject, attribute));
				}
			});
		}
		catch (Exception ex)
		{
			throw ex.InnerException ?? ex;
		}

		return results;
	}

	private static List<string> ValidateCollectionArgument(Array? elements, CommandParameter parameter)
	{
		var validationResults = new List<string>();

		if (parameter.MinLength > 0 && (elements is null || elements.Length < parameter.MinLength))
		{
			validationResults.Add($"Element count of '{parameter.Name}' must be at least {parameter.MinLength}");
		}

		return validationResults;
	}

	private static List<string> ValidateArgumentElement(object? value, CommandParameter parameter)
	{
		var validationResults = new List<string>();

		if (value is not null)
		{
			var isNumber = value.GetType().GetInterface("INumber`1") is not null && value is not char;
			var v = value is int p ? (double)p : value;

			if (parameter.MaxValue is var max && max is not null)
			{
				var m = max is int i ? (double)i : max;

				if (m.CompareTo(v) < 0)
				{
					validationResults.Add($"Value for '{parameter.Name}' must {(isNumber ? "be less then" : "preceed")} '{max}'");
				}
			}

			if (parameter.MinValue is var min && min is not null)
			{
				var m = min is int i ? (double)i : min;

				if (m.CompareTo(v) > 0)
				{
					validationResults.Add($"Value for '{parameter.Name}' must {(isNumber ? "be greater then" : "exceed")} '{min}'");
				}
			}

			if (v is string vs && vs is not null)
			{
				if (vs.Length < parameter.MinLength)
				{
					validationResults.Add($"Value for {parameter.Name} must be at least {parameter.MinLength} long");
				}
			}
		}

		return validationResults;
	}

	private Array? ParseArrayValues(List<CommandParameter> parameters, int index, Type arrayType)
	{
		if (index < Arguments?.Count)
		{
			var rest = Arguments.GetRange(index, Arguments.Count - index);

			var type = arrayType.GetElementType()!;

			var typedArray = Array.CreateInstance(type, rest.Count);

			Array.Copy(rest.Select(a => ContentParser.ParseStringValue(type, a)).ToArray(), typedArray, rest.Count);

			return typedArray;
		}
		else
		{
			return (Array?)parameters[index].DefaultValue;
		}
	}

	public MethodInfo GetMethod(Type type)
	{
		var methods = GetMethods(type);

		if (methods.TryGetValue(Reference?.MethodName.ToLower()!, out var method))
		{
			return method;
		}
		else
		{
			throw new KeyNotFoundException($"Method '{Reference}' not found on object type of '{type}'");
		}
	}

	public static Dictionary<string, MethodInfo> GetMethods(Type type)
	{
		return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.Union(type.GetMethods(BindingFlags.Public | BindingFlags.Static))
			.Select(m =>
				(attribute: m.GetCustomAttribute<CommandFunctionAttribute>(false), method: m)).Where(e =>
				e.attribute is not null).ToDictionary(k => k.attribute!.MethodReference.ToLower(), v => v.method);
	}

	public static List<CommandParameter> GetParameters(MethodInfo method)
	{
		return method.GetParameters().Select(p => new CommandParameter(p)).ToList();
	}

	public static CommandReference GetCommandReference(string rawCommand, out List<string> arguments)
	{
		var keys = rawCommand.Split(' ').ToList();

		if (keys.Count == 0)
		{
			throw new IncompleteCommandException(rawCommand, CommandTree.Commands.Keys.ToList());
		}

		var methodName = GetReferenceMethodNameRecursive(CommandTree.Commands, keys.First(), keys, rawCommand, out arguments);

		return new CommandReference(keys.First(), methodName);
	}

	private static string GetReferenceMethodNameRecursive(object obj, string className, List<string> keys, string rawCommand, out List<string> arguments)
	{
		if (obj is ExpandoObject o)
		{
			if (keys.FirstOrDefault() == SmartConsole.HELP_COMMAND)
			{
				throw new HelpRequestedException(rawCommand, o.Select(e => (e.Key, e.Value is not ExpandoObject)).ToList(), null);
			}
			else
			{
				var key = keys.First();

				if (string.IsNullOrWhiteSpace(key))
				{
					return GetReferenceMethodNameRecursive(obj, className, keys.GetRange(1, keys.Count - 1), rawCommand, out arguments);
				}
				else
				{
					var matchingValue = o.FirstOrDefault(e =>
						e.Key.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Contains(key, StringComparer.OrdinalIgnoreCase)).Value;

					var partialMatchingValue = o.FirstOrDefault(e =>
						e.Key.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Any(p => p.StartsWith(key, StringComparison.OrdinalIgnoreCase))).Value;

					if (matchingValue is null)
					{
						if (partialMatchingValue is not null)
						{
							throw new PartialKeyException(rawCommand, key);
						}
						else
						{
							throw new CommandKeyNotFoundException(rawCommand, key);
						}
					}

					if (matchingValue is ExpandoObject && keys.Count <= 1 || matchingValue is not ExpandoObject && keys.Count < 1)
					{
						throw new IncompleteCommandException(rawCommand, (matchingValue as ExpandoObject)?.Select(e => e.Key).ToList());
					}

					return GetReferenceMethodNameRecursive(matchingValue, className, keys.GetRange(1, keys.Count - 1), rawCommand, out arguments);
				}
			}
		}
		else
		{
			if (keys.FirstOrDefault() == SmartConsole.HELP_COMMAND)
			{
				throw new HelpRequestedException(rawCommand, null, new CommandReference(className, obj.ToString()!));
			}
			else
			{
				arguments = StackMata.ProcessArguments(string.Join(' ', keys));

				return (string)obj;
			}
		}
	}
}