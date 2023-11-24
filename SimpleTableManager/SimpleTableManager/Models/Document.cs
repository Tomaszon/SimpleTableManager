using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[CommandInformation("Loading, saving and other document related commands")]
public partial class Document
{
	public Metadata Metadata { get; set; } = new Metadata();

	public List<Table> Tables { get; set; } = new List<Table>();

	public Document(Size tableSize)
	{
		Metadata = new Metadata();
		Tables.Clear();
		AddTable(tableSize);
	}

	public void Clear()
	{
		Metadata = new Metadata();
		Tables.Clear();
		AddTable(Settings.Current.DefaultTableSize);
	}

	public Table GetActiveTable(out int index)
	{
		var table = Tables.Single(t => t.IsActive);
		index = Tables.IndexOf(table);
		return table;
	}

	public Table GetActiveTable()
	{
		return GetActiveTable(out _);
	}

	private static string GetSaveFilePath(string fileName)
	{
		return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.json");
	}
}