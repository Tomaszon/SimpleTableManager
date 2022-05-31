using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTableManager.Models
{
	public class Metadata
	{
		public string Title { get; set; }

		public string Path { get; set; }

		public Dictionary<string, string> CustomProperties { get; set; } = new Dictionary<string, string>();

		public Metadata(string title, string path)
		{
			Title = title;
			Path = path;
		}

		public void ClearFileData()
		{
			Path = null;
		}
	}
}
