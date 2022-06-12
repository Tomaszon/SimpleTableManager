using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
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
			var index = Tables.IndexOf(Tables.FirstOrDefault(t => t.Name == name));

			Shared.Validate(() => index != -1, $"No table found with name {name}");

			ActivateTable(index);
		}

		[CommandReference]
		public void AddTable(Size size, string name = null)
		{
			name = name ?? $"Table{Tables.Count}";

			if (Tables.Any(t => t.Name == name))
			{
				throw new ArgumentException($"Can not create table with duplicate name '{name}'");
			}
			else
			{
				Tables.Add(new Table(name, size.Width, size.Height));

				ActivateTable(Tables.Count - 1);
			}
		}

		[CommandReference]
		public void Save()
		{
			if (Metadata.Path is null)
			{
				throw new IOException($"Specify a file name to save to with 'save as'");
			}
			else
			{
				Save(Metadata.Path, true);
			}
		}

		[CommandReference("saveAs")]
		public void Save(string fileName, bool overwrite = false)
		{
			fileName = GetSaveFilePath(fileName);

			try
			{
				if (File.Exists(fileName) && !overwrite)
				{
					throw new IOException($"File '{fileName}' already exists, set {nameof(overwrite)} to 'true' to force file save");
				}

				using var f = File.Create(fileName);
				using var sw = new StreamWriter(f);

				Metadata.Path = fileName;

				new JsonSerializer().Serialize(new JsonTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented, IndentChar = '\t' }, this);
			}
			catch (Exception ex)
			{
				Metadata.ClearFileData();

				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}

				throw ex;
			}
		}

		[CommandReference]
		public void Load(string fileName)
		{
			fileName = GetSaveFilePath(fileName);

			try
			{
				using var f = File.Open(fileName, FileMode.Open);
				using var sr = new StreamReader(f);

				var content = sr.ReadToEnd();

				JsonConvert.PopulateObject(content, this, new JsonSerializerSettings() { ContractResolver = new ClearPropertyContractResolver() });

				Metadata.Path = fileName;
			}
			catch (Exception ex)
			{
				Clear();

				throw ex;
			}
		}
	}
}