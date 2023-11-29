using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

public partial class Document
{
	[CommandReference]
	public void SetTitle(string title)
	{
		Metadata.Title = title;
	}

	[CommandReference("activateTableAt")]
	public void ActivateTable(int index)
	{
		Shared.Validate(() => index >= 0 && index < Tables.Count, $"Index is not in the needed range: [0, {Tables.Count - 1}]");

		Tables.ForEach(t => t.IsActive = false);
		Tables[index].IsActive = true;
	}

	[CommandReference]
	public void ActivateTable(string name)
	{
		var index = Tables.IndexOf(Tables.FirstOrDefault(t => t.Name == name)!);

		Shared.Validate(() => index != -1, $"No table found with name {name}");

		ActivateTable(index);
	}

	[CommandReference]
	public void AddTable(Size size, string? name = null)
	{
		name ??= $"Table{Tables.Count}";

		if (Tables.Any(t => t.Name == name))
		{
			throw new InvalidOperationException($"Can not create table with duplicate name '{name}'");
		}
		else
		{
			var table = new Table(name, size.Width, size.Height);

			Tables.Add(table);

			table.StateModifierCommandExecuted += OnStateModifierCommandExecuted;

			ActivateTable(Tables.Count - 1);
		}
	}

	[CommandReference(StateModifier = false)]
	public void Save()
	{
		if (Metadata.Path is null)
		{
			throw new InvalidOperationException($"Specify a file name to save to with 'save as'");
		}
		else
		{
			Save(Metadata.Path, true);
		}
	}

	[CommandReference("saveAs", StateModifier = false)]
	public void Save(string fileName, bool overwrite = false)
	{
		fileName = GetSaveFilePath(fileName);

		if (File.Exists(fileName) && !overwrite)
		{
			throw new InvalidOperationException($"File '{fileName}' already exists, set {nameof(overwrite)} to 'true' to force file save");
		}

		try
		{
			using var f = File.Create(fileName);
			using var sw = new StreamWriter(f);

			var serializer = new JsonSerializer
			{
				TypeNameHandling = TypeNameHandling.Auto,
			};

			Metadata.CreateTime ??= DateTime.Now;

			serializer.Serialize(new JsonTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented, IndentChar = '\t', }, this);

			GetMetaInfos(fileName);

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
	}

	[CommandReference(StateModifier = false)]
	public void Load(string fileName, bool confirm = false)
	{
		if (!IsSaved && !confirm)
		{
			throw new InvalidOperationException($"Document contains unsaved changes that will be lost! Set {nameof(confirm)} to 'true' to force file load");
		}

		fileName = GetSaveFilePath(fileName);

		try
		{
			using var f = File.Open(fileName, FileMode.Open);
			using var sr = new StreamReader(f);

			var serializer = new JsonSerializer
			{
				TypeNameHandling = TypeNameHandling.Auto,
				ContractResolver = new ClearPropertyContractResolver(),
			};

			serializer.Populate(new JsonTextReader(sr), this);

			GetMetaInfos(fileName);

			IsSaved = true;
		}
		catch (Exception ex)
		{
			Clear();

			throw new OperationCanceledException("Can not load document", ex);
		}
	}

	public void GetMetaInfos(string path)
	{
		var fileInfo = new FileInfo(path);
		Metadata.Path = path;
		Metadata.Size = fileInfo.Length;
	}
}