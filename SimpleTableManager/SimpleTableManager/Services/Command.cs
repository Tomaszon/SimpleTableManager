using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public class Command
	{
		public CommandReference Reference { get; set; }

		public List<string> Arguments { get; set; }

		public List<string> AvailableKeys { get; set; }

		public string RawCommand { get; set; }

		public static Command FromString(string rawCommand)
		{
			var reference = CommandTree.GetCommandReference(rawCommand, out var arguments);

			return new Command
			{
				Reference = reference,
				Arguments = arguments,
				RawCommand = rawCommand
			};
		}

		public void Execute(IEnumerable<object> instances)
		{
			foreach (var instance in instances)
			{
				var method = GetMethod(instance);
				var parameters = GetParameters(method);

				if (parameters.Count(p => !p.IsOptional) > Arguments.Count ||
					parameters.All(p => !p.IsArray) && parameters.Count < Arguments.Count)
				{
					throw new ParameterCountException(RawCommand, Reference);
				}

				List<object> parsedArguments = new List<object>();

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
						var value = i < Arguments.Count ? Shared.ParseStringValue(paramType, Arguments[i]) : parameters[i].DefaultValue;

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

		private Array ParseArrayValues(List<CommandParameter> parameters, int index, Type arrayType)
		{
			if (index < Arguments.Count)
			{
				var rest = Arguments.GetRange(index, Arguments.Count - index);

				var type = arrayType.GetElementType();

				var typedArray = Array.CreateInstance(type, rest.Count);

				Array.Copy(rest.Select(a => Shared.ParseStringValue(type, a)).ToArray(), typedArray, rest.Count);

				return typedArray;
			}
			else
			{
				return (Array)parameters[index].DefaultValue;
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
			{
				var isArray = p.ParameterType.IsArray;

				return new CommandParameter
				{
					Type = p.ParameterType,
					Name = p.Name,
					IsOptional = p.IsOptional || isArray,
					DefaultValue = isArray ? Array.CreateInstance(p.ParameterType.GetElementType(), 0) : p.DefaultValue,
					IsArray = isArray,
					ParseFormat = Shared.GetParseMethod(isArray ? p.ParameterType.GetElementType() : p.ParameterType)?.GetCustomAttribute<ParseFormatAttribute>()?.Format
				};
			}).ToList();
		}

		public static bool IsValid(string rawCommand)
		{
			try
			{
				Command.FromString(rawCommand);

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
