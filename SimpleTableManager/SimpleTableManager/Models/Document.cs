using System.Collections.Generic;
using System.Linq;

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
			Tables.Add(new Table("Table1", 12, 8) { /*ViewOptions = new ViewOptions(3, 2, 6, 7)*/ });

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
	}
}
