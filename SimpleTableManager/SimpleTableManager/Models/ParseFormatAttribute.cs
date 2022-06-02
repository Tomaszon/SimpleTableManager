using System;

namespace SimpleTableManager.Models
{
	//TODO multiple formats?
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class ParseFormatAttribute : Attribute
	{
		public string Format { get; set; }

		public ParseFormatAttribute(string format)
		{
			Format = format;
		}
	}
}
