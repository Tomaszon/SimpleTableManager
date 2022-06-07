﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Document
	{
		public Metadata Metadata { get; set; }

		public List<Table> Tables { get; set; } = new List<Table>();

		public Document()
		{
			Clear();
		}

		public void Clear()
		{
			Metadata = new Metadata();
			Tables.Clear();
			AddTable(new Size(10, 4));
		}

		public Table GetActiveTable()
		{
			return Tables.Single(t => t.IsActive);
		}

		private static string GetSaveFilePath(string fileName)
		{
			return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.json");
		}
	}
}