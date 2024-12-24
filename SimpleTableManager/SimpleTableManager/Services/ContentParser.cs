namespace SimpleTableManager.Services;

public static class ContentParser
{
	public static Type GetTypeByFriendlyName(string name, string? nameSpace = null)
	{
		if (Shared.FRIENDLY_TYPE_NAMES.TryGetValue(name.ToLower(), out var mapped))
		{
			return Type.GetType(mapped, true, true)!;
		}

		return Type.GetType($"{nameof(System)}.{name}".ToLower(), false, true) ??
			Type.GetType($"{nameof(SimpleTableManager)}.{nameof(Models)}.{name}", true, true)!;
	}

	//REWORK use IParsable interface
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

	//REWORK use IParsable interface
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

	public static List<IFunctionArgument> ParseFunctionArguments<TType>(IEnumerable<string> values)
	where TType : IParsable<TType>
	{
		return values.Select(ParseFunctionArgument<TType>).ToList();
	}

	public static IFunctionArgument ParseFunctionArgument<TType>(string value)
	where TType : IParsable<TType>
	{
		if (CellReference.TryParse(value, null, out var cellReference))
		{
			return new ReferenceFunctionArgument(cellReference);
		}

		return new ConstFunctionArgument<TType>(TType.Parse(value, null));
	}
}