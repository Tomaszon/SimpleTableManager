namespace SimpleTableManager.Services;

public class ConvertibleJsonConverter : JsonConverter<IConvertible>
{
	public override IConvertible? ReadJson(JsonReader reader, Type objectType, IConvertible? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (objectType == typeof(string))
		{
			return (string?)reader.Value;
		}
		else if (reader.ValueType == typeof(string))
		{
			if (reader.Value is null)
			{
				return null;
			}

			return (IConvertible?)ContentParser.ParseConstStringValue(objectType, (string)reader.Value);
		}
		else
		{
			return (IConvertible?)((IConvertible?)reader.Value)?.ToType(objectType, null);
		}
	}

	public override void WriteJson(JsonWriter writer, IConvertible? value, JsonSerializer serializer)
	{
		throw new NotSupportedException();
	}
}