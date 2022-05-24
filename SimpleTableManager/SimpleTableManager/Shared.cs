using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleTableManager.Services
{
	public static class Shared
	{
		public static Type GetTypeByName(string name)
		{
			Dictionary<string, string> nameMap = new Dictionary<string, string>
			{
				{ "int", "int32" },
				{ "long", "int64" }
			};

			var type = nameMap.TryGetValue(name.ToLower(), out var mapped) ? mapped : name.ToLower();

			return Type.GetType($"system.{type}", true, true);
		}

		public static object ParseStringValue(Type dataType, string value)
		{
			if (dataType == typeof(string) || dataType == typeof(object))
			{
				return value;
			}

			var method = GetParseMethod(dataType.IsEnum ? typeof(Enum) : dataType);

			if (method is null)
			{
				throw new Exception($"Type '{dataType.Name}' does not have 'Parse' method");
			}

			try
			{
				return method.Invoke(null, dataType.IsEnum ? new object[] { dataType, value, true } : new object[] { value });
			}
			catch (Exception)
			{
				throw new FormatException($"Can not format value '{value}' to type '{dataType.Name}'");
			}
		}

		private static MethodInfo GetParseMethod(Type dataType)
		{
			return dataType.GetMethods().Where(m =>
			{
				var parameters = m.GetParameters();
				if (dataType == typeof(Enum))
				{
					return m.Name == "Parse" && parameters.Length == 3 &&
						parameters[0].ParameterType == typeof(Type) &&
						parameters[1].ParameterType == typeof(string) &&
						parameters[2].ParameterType == typeof(bool);
				}
				else
				{
					return m.Name == "Parse" && parameters.Length == 1;
				}
			}).SingleOrDefault();
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
			if (!validator())
			{
				throw new InvalidOperationException(error);
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
	}
}
