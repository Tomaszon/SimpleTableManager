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

		public static Command FromString(string value)
		{
			var reference = CommandTree.GetCommandReference(value, out var arguments, out var availableKeys);

			return new Command
			{
				Reference = reference,
				Arguments = arguments,
				AvailableKeys = availableKeys
			};
		}

		public void Execute(IEnumerable<object> instances)
		{
			foreach (var instance in instances)
			{
				var method = GetMethod(instance);
				var methodParameters = GetParameters(method);

				List<object> parsedArguments = new List<object>();

				for (var i = 0; i < methodParameters.Count; i++)
				{
					var paramType = methodParameters[i].Type;

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
						parsedArguments.Add(Shared.ParseStringValue(paramType, Arguments[i]));
					}
				}

				try
				{
					method.Invoke(instance, parsedArguments.ToArray());
				}
				catch (TargetParameterCountException)
				{
					var parameters = GetParameters(method);

					throw new TargetParameterCountException($"Command parameters needed: '{string.Join(", ", parameters)}'");
				}
				catch (Exception ex)
				{
					throw ex.InnerException ?? ex;
				}
			}
		}

		public MethodInfo GetMethod(object instance)
		{
			var methods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).ToList();

			var method = methods.SingleOrDefault(m =>
			{
				var attribute = m.GetCustomAttributes(typeof(CommandReferenceAttribute), false).FirstOrDefault() as CommandReferenceAttribute;

				return attribute is { } &&
					(attribute.MethodReference?.ToLower() == Reference.MethodName.ToLower() ||
					m.Name.ToLower() == Reference.MethodName.ToLower());
			});

			if (method is null)
			{
				throw new KeyNotFoundException($"Method '{Reference}' not found on object type of '{instance.GetType()}'");
			}

			return method;
		}

		public List<CommandParameter> GetParameters(MethodInfo method)
		{
			return method.GetParameters().Select(p =>
				new CommandParameter
				{
					Type = p.ParameterType,
					Name = p.Name,
					Optional = p.IsOptional
				}).ToList();
		}
	}

}
