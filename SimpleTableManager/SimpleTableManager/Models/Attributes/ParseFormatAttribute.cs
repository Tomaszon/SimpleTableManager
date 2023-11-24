namespace SimpleTableManager.Models.Attributes
{
	//TODO multiple formats?
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ParseFormatAttribute : Attribute
	{
		public string Format { get; set; }

		public string Regex { get; set; }

		public ParseFormatAttribute(string formatInfo, string regex)
		{
			Format = formatInfo;
			Regex = regex;
		}
	}
}
