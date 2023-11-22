namespace SimpleTableManager.Models.Attributes
{
	//TODO multiple formats?
	//TODO move to class
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
