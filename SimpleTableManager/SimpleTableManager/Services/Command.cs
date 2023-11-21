using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
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
			var reference = CommandTree.GetCommandReference(rawCommand, out var arguments);

			return new Command(reference, rawCommand, arguments);
		}

		public List<object?> Execute(IEnumerable<object> instances)
		{
			List<object?> results = new();

			foreach (var instance in instances)
			{
				var method = GetMethod(instance.GetType());
				var parameters = GetParameters(method);

				if (parameters.Count(p => !p.IsOptional) > Arguments?.Count ||
					parameters.All(p => !p.IsArray) && parameters.Count < Arguments?.Count)
				{
					throw new ParameterCountException(RawCommand, Reference);
				}

				List<object?> parsedArguments = new();

				for (var i = 0; i < parameters.Count; i++)
				{
					var paramType = parameters[i].Type;

					if (paramType.IsArray)
					{
						var values = ParseArrayValues(parameters, i, paramType);

						parsedArguments.Add(values);
					}
					else
					{
						var value = i < Arguments?.Count ?
							Shared.ParseStringValue(paramType, Arguments[i]) : parameters[i].DefaultValue;

						parsedArguments.Add(value);
					}
				}

				try
				{
					results.Add(method.Invoke(instance, parsedArguments.ToArray()));
				}
				catch (Exception ex)
				{
					throw ex.InnerException ?? ex;
				}
			}

			return results;
		}

		private Array? ParseArrayValues(List<CommandParameter> parameters, int index, Type arrayType)
		{
			if (index < Arguments?.Count)
			{
				var rest = Arguments.GetRange(index, Arguments.Count - index);

				var type = arrayType.GetElementType()!;

				var typedArray = Array.CreateInstance(type, rest.Count);

				Array.Copy(rest.Select(a => Shared.ParseStringValue(type, a)).ToArray(), typedArray, rest.Count);

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
			return type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(m =>
				(attribute: m.GetCustomAttribute<CommandReferenceAttribute>(false), method: m)).Where(e =>
					e.attribute is not null).ToDictionary(k => k.attribute!.MethodReference.ToLower(), v => v.method);
		}

		public List<CommandParameter> GetParameters(MethodInfo method)
		{
			return method.GetParameters().Select(p =>
			{
				var isArray = p.ParameterType.IsArray;

				return new CommandParameter
				(p.ParameterType, p.Name!)
				{
					DefaultValue = isArray ?
						Array.CreateInstance(p.ParameterType.GetElementType()!, 0) : p.DefaultValue,
					IsOptional = p.IsOptional || isArray,
					ParseFormat = Shared.GetParseMethod(isArray ? p.ParameterType.GetElementType()! :
						p.ParameterType, out _).GetCustomAttribute<ParseFormatAttribute>()?.Format
				};
			}).ToList();
		}

		public static bool IsValid(string rawCommand)
		{
			try
			{
				FromString(rawCommand);

				return true;
			}
			catch (Exception ex) when (ex is HelpRequestedException || ex is IncompleteCommandException)
			{
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
