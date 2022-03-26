using System;

namespace SimpleTableManager.Models
{
	public class CommandReferenceAttribute : Attribute
	{
		public string MethodReference { get; set; }

		public CommandReferenceAttribute(string reference = null)
		{
			MethodReference = reference;
		}
	}
}
