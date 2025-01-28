namespace SimpleTableManager.Services;

public class AppVersionConverter : VersionConverter
{
	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		if (Settings.Current.CheckAppVersionOnDocumentLoad && reader.Path == $"{nameof(Metadata)}.{nameof(Metadata.AppVersion)}")
		{
			if (reader.Value is not null && Shared.GetAppVersion() == new Version(reader.Value.ToString()!))
			{
				return base.ReadJson(reader, objectType, existingValue, serializer);
			}
			else
			{
				throw new JsonSerializationException("Unsupported document version");
			}
		}
		else
		{
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}
	}
}