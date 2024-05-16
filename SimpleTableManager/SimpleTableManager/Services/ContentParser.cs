namespace SimpleTableManager.Services;

public static class ContentParser
{
	public static Type GetTypeByFriendlyName(string name, string? nameSpace = null)
	{
		var type = Shared.FRIENDLY_TYPE_NAMES.TryGetValue(name.ToLower(), out var mapped) ? mapped : name.ToLower();

		return Type.GetType($"{nameSpace ?? "system"}.{type}", true, true)!;
	}

	public static object ParseStringValue(Type dataType, string value)
	{
		if (dataType == typeof(string) || dataType == typeof(object))
		{
			return value;
		}

		if (GetParseMethod(dataType, out var targetDataType) is var method && method is null)
		{
			throw new Exception($"Type '{Shared.FormatTypeName(targetDataType)}' does not have 'Parse' method");
		}

		try
		{
			if (dataType.Name == "Nullable`1" && dataType.GenericTypeArguments[0].IsEnum || dataType.IsEnum)
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
			var attributes = targetDataType.GetCustomAttributes<ParseFormatAttribute>();

			throw new FormatException($"Can not format value '{value}' to type '{dataType.GetFriendlyName()}'{(attributes.Any() ? $" Required formats: '{string.Join("' '", attributes.Select(a => a.Format))}'" : "")}", ex);
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

		var type = GetTypeByFriendlyName(dataTypeName);

		return ParseStringValue(type, value);
	}
}
