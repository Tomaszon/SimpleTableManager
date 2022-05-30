using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class Document
	{
		public Metadata Metadata { get; set; }

		public List<Table> Tables { get; set; } = new List<Table>();

		public Document()
		{
			Metadata = new Metadata("", 0, "");
			Tables.Add(new Table("Table1", 10, 4) { /*ViewOptions = new ViewOptions(3, 2, 6, 7)*/ });

			ActivateTable(0);
		}

		public Table GetActiveTable()
		{
			return Tables.Single(t => t.IsActive);
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
			var index = Tables.IndexOf(Tables.First(t => t.Name == name));

			Shared.Validate(() => index != -1, $"No table found with name {name}");

			ActivateTable(index);
		}

		[CommandReference]
		public void AddTable(int columnCount, int rowCount, string name = "table1")
		{
			Tables.Add(new Table(name, columnCount, rowCount));

			ActivateTable(Tables.Count - 1);
		}

		[CommandReference]
		public void Save(string fileName, bool overwrite = false)
		{
			fileName = GetSaveFilePath(fileName);

			if (File.Exists(fileName) && !overwrite)
			{
				throw new IOException($"File '{fileName}' already exists, set {nameof(overwrite)} to 'true' to force file save");
			}

			using var f = File.Create(fileName);
			using var sw = new StreamWriter(f);

			new JsonSerializer().Serialize(new JsonTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented, IndentChar = '\t' }, this);
		}

		[CommandReference]
		public void Load(string fileName)
		{
			fileName = GetSaveFilePath(fileName);

			using var f = File.Open(fileName, FileMode.Open);
			using var sr = new StreamReader(f);

			var content = sr.ReadToEnd();

			JsonConvert.PopulateObject(content, this, new JsonSerializerSettings() {  });
		}

		private static string GetSaveFilePath(string fileName)
		{
			return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.json");
		}
	}
}
