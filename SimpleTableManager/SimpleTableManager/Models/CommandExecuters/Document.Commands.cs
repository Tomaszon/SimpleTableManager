using Newtonsoft.Json.Linq;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Document
{
	[CommandFunction]
	public void SetTitle(string title)
	{
		Metadata.Title = title;
	}

	[CommandFunction("activateTableAt")]
	public void ActivateTable(int index)
	{
		ThrowIfNot(index >= 0 && index < Tables.Count, $"Index is not in the needed range: [0, {Tables.Count - 1}]");

		Tables.ForEach(t => t.IsActive = false);
		Tables[index].IsActive = true;
	}

	[CommandFunction]
	public void ActivateTable(string name)
	{
		var index = Tables.IndexOf(Tables.FirstOrDefault(t => t.Name == name)!);

		ThrowIf(index == -1, $"No table found with name {name}");

		ActivateTable(index);
	}

	[CommandFunction, CommandShortcut]
	public void ActivateNextTable()
	{
		GetActiveTable(out var index);

		if (index < Tables.Count - 1)
		{
			ActivateTable(index + 1);
		}
	}

	[CommandFunction, CommandShortcut]
	public void ActivatePreviousTable()
	{
		GetActiveTable(out var index);

		if (index > 0)
		{
			ActivateTable(index - 1);
		}
	}

	[CommandFunction, CommandShortcut]
	public void AddNewTable(Size? size = null, string? name = null)
	{
		name ??= $"Table{Tables.Count}";
		size ??= Settings.Current.DefaultTableSize;

		ThrowIf(Tables.Any(t => t.Name == name), $"Can not create table with duplicate name '{name}'");

		var table = new Table(name, size.Width, size.Height);

		Tables.Add(table);

		table.StateModifierCommandExecuted += OnStateModifierCommandExecuted;

		ActivateTable(Tables.Count - 1);
	}

	[CommandInformation("Saves the document overwriting the previous save file")]
	[CommandShortcut("saveDocument"), CommandFunction(StateModifier = false)]
	public void Save()
	{
		ThrowIf(Metadata.Path is null, $"Specify a file name to save to with 'save-as'");

		Save(Metadata.Path, true);
	}

	[CommandFunction("saveAs", StateModifier = false)]
	public void Save(string fileName, bool overwrite = false)
	{
		fileName = Shared.GetWorkFilePath(fileName, "json");

		if (File.Exists(fileName) && !overwrite)
		{
			throw new InvalidOperationException($"File '{fileName}' already exists, set {nameof(overwrite)} to 'true' to force file save");
		}

		try
		{
			using var f = File.Create(fileName);
			using var sw = new StreamWriter(f);

			Metadata.CreateTime ??= DateTime.Now;

			Serialize(sw);

			IsSaved = true;
		}
		catch (Exception ex)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			throw new OperationCanceledException("Can not save document", ex);
		}

		GetMetaInfos(fileName);
	}

	[CommandFunction(StateModifier = false)]
	[CommandInformation<Document>]
	public void Load(string fileName, bool confirm = false)
	{
		ThrowIf(IsSaved == false && !confirm, T("unsaved", nameof(confirm)));

		fileName = Shared.GetWorkFilePath(fileName, "json");

		var app = InstanceMap.Instance.GetInstance<App>()!;

		try
		{
			using var f = File.Open(fileName, FileMode.Open);
			using var sr = new StreamReader(f);

			Deserialize(sr);

			IsSaved = true;

			app.EditHistory.Clear();
		}
		catch (Exception ex)
		{
			app.Undo();

			throw new OperationCanceledException("Can not load document", ex);
		}

		GetMetaInfos(fileName);
	}

	[CommandFunction(StateModifier = false)]
	[CommandShortcut("exportDocument"), CommandInformation("Exports each table as a CSV file")]
	public void Export(bool overwrite = false)
	{
		Tables.ForEach(t =>
		{
			var fileName = Shared.GetWorkFilePath($"{Metadata.Title}.{t.Name}", "csv");

			ThrowIf<InvalidOperationException>(File.Exists(fileName) && !overwrite, $"File '{fileName}' already exists, set {nameof(overwrite)} to 'true' to force file save");
		});

		Tables.ForEach(t =>
			t.Export(Shared.GetWorkFilePath($"{Metadata.Title}.{t.Name}", "csv"), overwrite));
	}

	[CommandFunction(StateModifier = false)]
	[CommandInformation("List all available valid json files that might be STM documents")]
	public object List(string pattern = "*")
	{
		string fileNames = string.Empty;
		var files = Directory.GetFiles(Settings.Current.DefaultWorkDirectory, $"{pattern}.json");

		files.ForEach(f =>
		{
			try
			{
				_ = JToken.Parse(File.ReadAllText(f));

				fileNames += $"{Path.GetFileNameWithoutExtension(f)}|";
			}
			catch { }
		});

		return new
		{
			Directory = Settings.Current.DefaultWorkDirectory,
			Files = fileNames.TrimEnd('|')
		};
	}

	[CommandFunction]
	public void Close(bool confirm = false)
	{
		ThrowIf(IsSaved == false && !confirm, $"Document contains unsaved changes that will be lost! Set {nameof(confirm)} to 'true' to force document close");

		Clear();

		InstanceMap.Instance.GetInstance<App>()!.EditHistory.Clear();
	}
}