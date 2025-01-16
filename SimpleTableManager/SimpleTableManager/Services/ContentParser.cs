using System.ComponentModel;
using System.Globalization;

namespace SimpleTableManager.Services;

public static class ContentParser
{
	public static Array? ParseStringValues(Type arrayType, List<string> values, Type? constArgumentValueType)
	{
		var elementType = arrayType.GetElementType() ?? arrayType.GenericTypeArguments.Single();

		var typedArray = Array.CreateInstance(elementType, values.Count);

		Array.Copy(values.Select(a => ParseStringValue(elementType, a, constArgumentValueType)).ToArray(), typedArray, values.Count);

		return typedArray;
	}

	public static object ParseStringValue(Type dataType, string value, Type? constArgumentValueType)
	{
		//IDEA make it generic for other interfaces?
		if (dataType == typeof(IFunctionArgument) && ReferenceFunctionArgument.TryParse(value, null, out var referenceFunctionArgument))
		{
			return referenceFunctionArgument;
		}
		else
		{
			var targetType = constArgumentValueType ?? dataType;

			var converter = targetType.Namespace == $"{nameof(SimpleTableManager)}.{nameof(Models)}" ?
				(TypeConverter)Activator.CreateInstance(typeof(ParsableStringConverter<>).MakeGenericType(targetType))! :
				TypeDescriptor.GetConverter(targetType);

			var result = converter.ConvertFromString(null, CultureInfo.CurrentUICulture, value);

			if (dataType == typeof(IFunctionArgument) && constArgumentValueType is not null)
			{
				return Activator.CreateInstance(typeof(ConstFunctionArgument<>).MakeGenericType(constArgumentValueType), result)!;
			}

			return result!;
		}
	}

	public static List<IFunctionArgument> ParseFunctionArguments<TType>(IEnumerable<string> values)
	where TType : IParsable<TType>
	{
		return [.. values.Select(v => ParseFunctionArgument<TType>(v))];
	}

	public static IFunctionArgument ParseFunctionArgument<TType>(string value)
	where TType : IParsable<TType>
	{

		if (ReferenceFunctionArgument.TryParse(value, null, out var referenceFunctionArgument))
		{
			return referenceFunctionArgument;
		}

		return new ConstFunctionArgument<TType>(TType.Parse(value, CultureInfo.CurrentUICulture));
	}
}