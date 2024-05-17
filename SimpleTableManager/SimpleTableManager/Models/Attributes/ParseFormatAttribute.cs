namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
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