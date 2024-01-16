using System.Runtime.Serialization;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[CommandInformation("Loading, saving and other document related commands")]
public partial class Document : CommandExecuterBase
{
	[JsonIgnore]
	public bool? IsSaved { get; set; }

	public Metadata Metadata { get; set; }

	public List<Table> Tables { get; set; } = new();

	public Document(Size tableSize)
	{
		Metadata = new();
		Tables.Clear();
		AddNewTable(tableSize);
	}

	public override void OnStateModifierCommandExecuted()
	{
		if (Settings.Current.Autosave && Metadata.Path is not null)
		{
			Save();
		}
		else
		{
			IsSaved = false;
		}

		InvokeStateModifierCommandExecutedEvent();
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext _)
	{
		Tables.ForEach(t => t.StateModifierCommandExecuted += OnStateModifierCommandExecuted);
	}

	public void Clear()
	{
		Metadata = new Metadata();
		Tables.Clear();
		AddNewTable(Settings.Current.DefaultTableSize);
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

	public void Serialize(StreamWriter sw)
	{
		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
		};

		serializer.Serialize(new JsonTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented, IndentChar = '\t', }, this);
	}

	public void Deserialize(StreamReader sr)
	{
		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new ClearPropertyContractResolver(),
		};

		serializer.Populate(new JsonTextReader(sr), this);
	}

	public void GetMetaInfos(string path)
	{
		var fileInfo = new FileInfo(path);
		Metadata.Path = path;
		Metadata.Size = fileInfo.Length;
	}
}