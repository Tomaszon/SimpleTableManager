using System.Collections.Generic;

namespace SimpleTableManager.Models
{
	public class Document
	{
		public Metadata Metadata { get; set; }

		public List<Table> Tables { get; set; } = new List<Table>();
	}
}
