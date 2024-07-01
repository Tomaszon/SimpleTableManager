using System.Runtime.Serialization;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Loading, saving and other document related commands")]
public partial class Document : CommandExecuterBase
{
	[JsonIgnore]
	public bool? IsSaved { get; set; }

	public Metadata Metadata { get; set; }

	public List<Table> Tables { get; set; } = new();

	public Document(Size tableSize)
	{
		Clear(tableSize);
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

	[MemberNotNull(nameof(Metadata))]
	public void Clear(Size? size = null)
	{
		Metadata = new Metadata();
		Tables.Clear();
		AddNewTable(size ?? Settings.Current.DefaultTableSize);
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

	public void Serialize(StreamWriter sw)
	{
		Shared.SerializeObject(sw, this, TypeNameHandling.Auto);
	}

	public void Deserialize(StreamReader sr)
	{
		Shared.DeserializeObject(sr, this);
	}

	public void GetMetaInfos(string path)
	{
		var fileInfo = new FileInfo(path);
		Metadata.Path = path;
		Metadata.Size = fileInfo.Length;
	}
}