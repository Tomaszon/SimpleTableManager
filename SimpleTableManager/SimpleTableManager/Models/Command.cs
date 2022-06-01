using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class Command
	{
		public CommandReference Reference { get; set; }

		public List<string> Arguments { get; set; }

		public List<string> AvailableKeys { get; set; }

		public string RawCommand { get; set; }

		public static Command FromString(string value)
		{
			var reference = CommandTree.GetCommandReference(value, out var arguments, out var availableKeys);

			return new Command
			{
				Reference = reference,
				Arguments = arguments,
				AvailableKeys = availableKeys,
				RawCommand = value
			};
		}

		public void Execute(IEnumerable<object> instances)
		{
			foreach (var instance in instances)
			{
				var method = GetMethod(instance);
				var parameters = GetParameters(method);

				if (parameters.Count(p => !p.Optional) > Arguments.Count || parameters.Count < Arguments.Count)
				{
					throw new TargetParameterCountException();
				}

				List<object> parsedArguments = new List<object>();

				for (var i = 0; i < parameters.Count; i++)
				{
					var paramType = parameters[i].Type;

					if (paramType.IsArray)
					{
						var rest = Arguments.GetRange(i, Arguments.Count - i);

						var type = Shared.GetTypeByName(paramType.Name.TrimEnd('[', ']'));

						var typedArray = Array.CreateInstance(type, rest.Count);

						Array.Copy(rest.Select(a => Shared.ParseStringValue(type, a)).ToArray(), typedArray, rest.Count);

						parsedArguments.Add(typedArray);

						break;
					}
					else
					{
						var value = i < Arguments.Count ?
							Shared.ParseStringValue(paramType, Arguments[i]) : parameters[i].DefaultValue;

						parsedArguments.Add(value);
					}
				}

				try
				{
					method.Invoke(instance, parsedArguments.ToArray());
				}
				catch (Exception ex)
				{
					throw ex.InnerException ?? ex;
				}
			}
		}

		public MethodInfo GetMethod(object instance)
		{
			var methods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(m =>
			(attribute: m.GetCustomAttribute<CommandReferenceAttribute>(false), method: m)).Where(e =>
				e.attribute is not null).ToDictionary(k => k.attribute.MethodReference.ToLower(), v => v.method);

			if (methods.TryGetValue(Reference.MethodName.ToLower(), out var method))
			{
				return method;
			}
			else
			{
				throw new KeyNotFoundException($"Method '{Reference}' not found on object type of '{instance.GetType()}'");
			}
		}

		public List<CommandParameter> GetParameters(MethodInfo method)
		{
			return method.GetParameters().Select(p =>
				new CommandParameter
				{
					Type = p.ParameterType,
					Name = p.Name,
					Optional = p.IsOptional,
					DefaultValue = p.DefaultValue
				}).ToList();
		}
	}

}
