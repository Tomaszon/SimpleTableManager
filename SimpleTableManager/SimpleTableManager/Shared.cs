using System.Text.RegularExpressions;

namespace SimpleTableManager.Services
{
	public static class Shared
	{
		public const string HELP_COMMAND = "help";

		public static Type GetTypeByName(string name, string? nameSpace = null)
		{
			Dictionary<string, string> nameMap = new()
			{
				{ "int", "int32" },
				{ "long", "int64" },
				{ "bool", "boolean" }
			};

			var type = nameMap.TryGetValue(name.ToLower(), out var mapped) ? mapped : name.ToLower();

			return Type.GetType($"{nameSpace ?? "system"}.{type}", true, true)!;
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
				if (dataType.IsEnum)
				{
					if (int.TryParse(value, out _))
					{
						throw new FormatException("Enum must be provided by name instead of value");
					}

					return method.Invoke(null, new object[] { targetDataType, value, true })!;
				}
				else
				{
					return method.Invoke(null, new object?[] { value, null })!;
				}
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
				var methods = dataType.IsEnum ? typeof(Enum).GetMethods() : dataType.GetInterfaceMap(dataType.GetInterface("IParsable`1")!).TargetMethods;

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
						return m.Name.EndsWith("Parse") &&
							m.Name != "TryParse" &&
							parameters.Length == 2 &&
							parameters[0].ParameterType == typeof(string);
					}
				}).Single();
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
				throw (T)Activator.CreateInstance(typeof(T), error)!;
			}
		}

		public static List<int> IndexArray(int size, int from = 0, int step = 1)
		{
			return Enumerable.Repeat(from, size).Select((value, index) => value + index * step).ToList();
		}

		public static object? GetDefaultValue(Type type)
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

		public static string ReadLineWhile(string message, IEnumerable<object> validValues)
		{
			do
			{
				Console.WriteLine(message);

				if (Console.ReadLine() is string answer && validValues.Select(v => v.ToString()).Contains(answer, StringComparer.OrdinalIgnoreCase))
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

		public static (Dictionary<ArgumentName, string>, IEnumerable<TType>) SeparateNamedArguments<TType>(params string[] arguments) where TType : IParsable<TType>
		{
			var namedArgs = arguments.Where(a => a.Contains(':') == true);

			var regularArgs = arguments.Where(a => !namedArgs.Contains(a)).Select(e => TType.Parse(e, null));

			var namedArgsDic = namedArgs.ToDictionary(k => Enum.Parse<ArgumentName>(k.Split(':')[0], true), v => v.Substring(v.IndexOf(':') + 1));

			return (namedArgsDic, regularArgs);
		}
	}
}
