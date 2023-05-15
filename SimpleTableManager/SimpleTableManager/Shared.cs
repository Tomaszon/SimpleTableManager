﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleTableManager.Models.Attributes;

namespace SimpleTableManager.Services
{
	public static class Shared
	{
		public const string HELP_COMMAND = "help";

		public static Type GetTypeByName(string name, string nameSpace = null)
		{
			Dictionary<string, string> nameMap = new Dictionary<string, string>
			{
				{ "int", "int32" },
				{ "long", "int64" }
			};

			var type = nameMap.TryGetValue(name.ToLower(), out var mapped) ? mapped : name.ToLower();

			return Type.GetType($"{nameSpace ?? "system"}.{type}", true, true);
		}

		public static object ParseStringValue(Type dataType, string value)
		{
			if (dataType == typeof(string) || dataType == typeof(object))
			{
				return value;
			}


			var method = GetParseMethod(dataType, out var targetDataType);

			if (method is null)
			{
				throw new Exception($"Type '{FormatTypeName(targetDataType)}' does not have 'Parse' method");
			}

			try
			{
				return method.Invoke(null, method.GetParameters().Length == 3 ? new object[] { targetDataType, value, true } : new object[] { value });
			}
			catch (Exception ex)
			{
				var attribute = method.GetCustomAttribute<ParseFormatAttribute>();

				throw new FormatException($"Can not format value '{value}' to type '{dataType.Name}'{(attribute is not null ? $" Required format: '{attribute.Format}'" : "")}", ex);
			}
		}

		public static MethodInfo GetParseMethod(Type dataType, out Type targetDataType)
		{
			if (dataType.Name == "Nullable`1")
			{
				return GetParseMethod(dataType.GenericTypeArguments[0], out targetDataType);
			}
			else
			{
				var methods = dataType.IsEnum ? typeof(Enum).GetMethods() : dataType.GetMethods();

				targetDataType = dataType;

				return methods.Where(m =>
				{
					var parameters = m.GetParameters();
					if (dataType.IsEnum)
					{
						return m.Name == "Parse" && parameters.Length == 3 &&
							parameters[0].ParameterType == typeof(Type) &&
							parameters[1].ParameterType == typeof(string) &&
							parameters[2].ParameterType == typeof(bool);
					}
					else
					{
						return m.Name == "Parse" && parameters.Length == 1 && parameters[0].ParameterType == typeof(string);
					}
				}).SingleOrDefault();
			}
		}

		public static object ParseStringValue(string dataTypeName, string value)
		{
			if (string.IsNullOrWhiteSpace(dataTypeName))
			{
				return value;
			}

			var type = GetTypeByName(dataTypeName);

			return ParseStringValue(type, value);
		}

		public static void Validate(Func<bool> validator, string error)
		{
			Validate<InvalidOperationException>(validator, error);
		}

		public static void Validate<T>(Func<bool> validator, string error) where T : Exception
		{
			if (!validator())
			{
				throw (T)Activator.CreateInstance(typeof(T), error);
			}
		}

		public static List<int> IndexArray(int size, int from = 0, int step = 1)
		{
			return Enumerable.Repeat(from, size).Select((value, index) => value + index * step).ToList();
		}

		public static object GetDefaultValue(Type type)
		{
			return type.IsValueType ? Activator.CreateInstance(type) : null;
		}

		public static int Max(params int[] values)
		{
			return Enumerable.Max(values);
		}

		public static int Min(params int[] values)
		{
			return Enumerable.Min(values);
		}

		public static void StepCursor(int x, int y)
		{
			Console.SetCursorPosition(Console.CursorLeft + x, Console.CursorTop + y);
		}

		public static string ReadLineWhile(string message, IEnumerable<object> validValues, bool caseSensitive = false)
		{
			do
			{
				Console.WriteLine(message);

				if (Console.ReadLine() is var answer && validValues.Select(v => v.ToString()).Contains(answer, StringComparer.OrdinalIgnoreCase))
				{
					return answer;
				}
			}
			while (true);
		}

		public static string FormatTypeName(Type type)
		{
			return type.IsGenericType ? $"{type.Name}({string.Join(',', type.GenericTypeArguments.Select(t => FormatTypeName(t)))})" : type.Name;
		}
	}
}
