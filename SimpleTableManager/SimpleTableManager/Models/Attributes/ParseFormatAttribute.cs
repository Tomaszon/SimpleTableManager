using SimpleTableManager.Services;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ParseFormatAttribute : Attribute
{
	public string Format { get; set; }

	public string Regex { get; set; }

	public ParseFormatAttribute(string formatInfo, string regex, object[]? args = null)
	{
		Format = args is null ? formatInfo : string.Format(formatInfo, args);
		Regex = args is null ? regex :  string.Format(regex, args);
	}
}