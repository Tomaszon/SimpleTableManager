using System.ComponentModel;
using System.Globalization;

namespace SimpleTableManager.Services;

public static class ContentParser
{
	public static Array? ParseStringValues(Type arrayType, List<string> values, Type? valueType)
	{
		var elementType = arrayType.GetElementType() ?? arrayType.GenericTypeArguments.Single();

		var typedArray = Array.CreateInstance(elementType, values.Count);

		Array.Copy(values.Select(a => ParseStringValue(elementType, a, valueType)).ToArray(), typedArray, values.Count);

		return typedArray;
	}

	public static object? ParseStringValue(Type dataType, string value, Type? valueType)
	{
		if (dataType == typeof(IFunctionArgument) && ReferenceFunctionArgument.TryParse(value, null, out var referenceFunctionArgument))
		{
			return referenceFunctionArgument;
		}
		else if (dataType == typeof(IFunctionArgument) && valueType is not null)
		{
			var genType = typeof(ConstFunctionArgument<>).MakeGenericType(valueType);

			//TODO change to nameof unbound generic in dotnet10
			var method = genType.GetMethod(nameof(ParsableBase<ConstFunctionArgument<int>>.TryParse), 
				BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)!;

			var args = new object?[] { value, null, null };

			if ((bool)method.Invoke(null, args)!)
			{
				return (IConstFunctionArgument)args[2]!;
			}
		}

		return ParseConstStringValue(dataType, value);
	}

	public static object? ParseConstStringValue(Type targetType, string value)
	{
		var converter = targetType.Namespace == $"{nameof(SimpleTableManager)}.{nameof(Models)}" ?
			(TypeConverter)Activator.CreateInstance(typeof(ParsableStringConverter<>).MakeGenericType(targetType))! :
			TypeDescriptor.GetConverter(targetType);

		return converter.ConvertFromString(null, CultureInfo.CurrentUICulture, value);
	}
}