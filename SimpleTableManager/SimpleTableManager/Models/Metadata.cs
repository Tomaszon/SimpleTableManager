namespace SimpleTableManager.Models
{
	public class Metadata
	{
		public string Title { get; set; } = "Document0";

		public string? Path { get; set; }

		public Dictionary<string, string> CustomProperties { get; set; } = new Dictionary<string, string>();

		public Dictionary<string, Type> CustomEnums { get; set; } = new Dictionary<string, Type>();

		public Metadata() { }

		public Metadata(string title, string path)
		{
			Title = title;
			Path = path;
		}

		public void ClearFileData()
		{
			Path = null;
		}
	}
}
