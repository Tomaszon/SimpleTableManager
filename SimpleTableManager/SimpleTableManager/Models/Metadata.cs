namespace SimpleTableManager.Models;

public class Metadata
{
	public string Author { get; set; } = Settings.Current.Author;

	public string Title { get; set; } = "Document0";

	[JsonIgnore]
	public string? Path { get; set; }

	[JsonIgnore]
	public long? Size { get; set; }

	public DateTime? CreateTime { get; set; }

	public Version AppVersion { get; set; } = Shared.GetAppVersion();

	public Dictionary<string, string> CustomProperties { get; set; } = [];
}