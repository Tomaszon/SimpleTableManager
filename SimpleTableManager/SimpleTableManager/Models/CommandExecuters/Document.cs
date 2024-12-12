using System.Runtime.Serialization;

using Newtonsoft.Json.Linq;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Loading, saving and other document related commands")]
[JsonObject(IsReference = true)]
public partial class Document : CommandExecuterBase
{
	[JsonIgnore]
	public bool? IsSaved { get; set; }

	public Metadata Metadata { get; set; }

	public List<Table> Tables { get; set; } = new();

	public GlobalStorage GlobalStorage { get; set; }

	public Document(Size tableSize)
	{
		Clear(tableSize);
	}

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, IStateModifierCommandExecuter root)
	{
		if (Settings.Current.Autosave && Metadata.Path is not null)
		{
			Save();
		}
		else
		{
			IsSaved = false;
		}

		InvokeStateModifierCommandExecutedEvent(root);
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext _)
	{
		Tables.ForEach(t => t.StateModifierCommandExecuted += OnStateModifierCommandExecuted);
	}

	[MemberNotNull(nameof(Metadata)), MemberNotNull(nameof(GlobalStorage))]
	public void Clear(Size? size = null)
	{
		Metadata = new Metadata();
		GlobalStorage = new GlobalStorage();
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

	public void GetMetaInfos(string path)
	{
		var fileInfo = new FileInfo(path);
		Metadata.Path = path;
		Metadata.Size = fileInfo.Length;
	}

	private bool CheckFileVersion(string content)
	{
		try
		{
			var obj = JObject.Parse(content);

			var value = (string?)obj.Descendants().Where(t =>
				t.Type == JTokenType.Property && t.Path == $"{nameof(Metadata)}.{nameof(Metadata.AppVersion)}").Select(t =>
					((JProperty)t).Value).SingleOrDefault();

			var version = value is not null ? new Version(value) : null;

			return Shared.GetAppVersion() == version;

		}
		catch
		{
			return false;
		}
	}
}