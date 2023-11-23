namespace SimpleTableManager.Models
{
	public class Metadata
	{
		public string Title { get; set; } = "Document0";

		[JsonIgnore]
		public string? Path { get; set; }

		public Dictionary<string, string> CustomProperties { get; set; } = new Dictionary<string, string>();

		public Metadata() { }
	}
}
