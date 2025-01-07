using System.ComponentModel;

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

	public static object ParseStringValue(Type dataType, string value)
	{
		var converter = dataType.Namespace == $"{nameof(SimpleTableManager)}.{nameof(Models)}" ?
			(TypeConverter)Activator.CreateInstance(typeof(ParsableStringConverter<>).MakeGenericType(dataType))! :
			TypeDescriptor.GetConverter(dataType);

		var result = converter.ConvertFromString(value);

		return result!;
	}

	public static List<IFunctionArgument> ParseFunctionArguments<TType>(Cell cell, IEnumerable<string> values)
	where TType : IParsable<TType>
	{
		return values.Select(v => ParseFunctionArgument<TType>(cell, v)).ToList();
	}

	public static IFunctionArgument ParseFunctionArgument<TType>(Cell cell, string value)
	where TType : IParsable<TType>
	{
		if (CellReference.TryParse(value, cell, out var cellReference))
		{
			return new ReferenceFunctionArgument(cellReference);
		}

		return new ConstFunctionArgument<TType>(TType.Parse(value, null));
	}
}