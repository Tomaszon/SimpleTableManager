using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleTableManager.Models
{
	public class Command
	{
		public string Reference { get; set; }

		public List<string> Parameters { get; set; }

		public List<string> AvailableKeys { get; set; }

		public static Command FromString(string value)
		{
			var reference = CommandTree.GetCommandReference(value, out var parameters, out var availableKeys);

			return new Command
			{
				Reference = reference,
				Parameters = parameters,
				AvailableKeys = availableKeys
			};
		}

		public void Execute(object instance)
		{
			var method = GetMethod(instance);

			var kvs = GetParameters(method).Zip(Parameters).ToDictionary(x => x.First, x => x.Second);

			var parsedParameters = kvs.Select(p => Services.Shared.ParseStringValue(p.Key.Type.Name, p.Value)).ToArray();

			try
			{
				method.Invoke(instance, parsedParameters);
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

		public MethodInfo GetMethod(object instance)
		{
			var methods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).ToList();

			var method = methods.SingleOrDefault(m =>
			{
				var attribute = m.GetCustomAttributes(typeof(CommandReferenceAttribute), false).FirstOrDefault() as CommandReferenceAttribute;

				return attribute is { } &&
					(attribute.Reference?.ToLower() == Reference.ToLower() || m.Name.ToLower() == Reference.ToLower());
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
