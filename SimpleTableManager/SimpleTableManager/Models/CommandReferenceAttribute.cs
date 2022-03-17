using System;

namespace SimpleTableManager.Models
{
	public class CommandReferenceAttribute : Attribute
	{
		public string Reference { get; set; }

		public CommandReferenceAttribute(string reference = null)
		{
			Reference = reference;
		}
	}
}
