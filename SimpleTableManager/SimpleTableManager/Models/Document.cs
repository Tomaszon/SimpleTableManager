using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	[CommandInformation("Loading, saving and other document related commands")]
	public partial class Document
	{
		public Metadata Metadata { get; set; } = new Metadata();

		public List<Table> Tables { get; set; } = new List<Table>();

		public Document()
		{
			Clear();
		}

		public void Clear()
		{
			Metadata = new Metadata();
			Tables.Clear();
			AddTable(new Size(10, 5));
		}

		public Table GetActiveTable()
		{
			return Tables.Single(t => t.IsActive);
		}

		private static string GetSaveFilePath(string fileName)
		{
			return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.json");
		}
	}
}