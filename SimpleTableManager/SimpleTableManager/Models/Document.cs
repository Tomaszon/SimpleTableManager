using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Models
{
	public class Document
	{
		public Metadata Metadata { get; set; }

		public List<Table> Tables { get; set; } = new List<Table>();

		public Document()
		{
			Metadata = new Metadata("", 0, "");
			Tables.Add(new Table("", 3, 4));

			ActivateTableAt(0);
		}

		public Table GetActiveTable()
		{
			return Tables.Single(t => t.IsActive);
		}

		public void ActivateTableAt(int index)
		{
			Tables.ForEach(t => t.IsActive = false);
			Tables[index].IsActive = true;
		}
	}
}
