namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ParseFormatAttribute(string formatInfo, string regex, object[]? args = null) : Attribute
{
	public string Format { get; set; } = args is null ? formatInfo : string.Format(formatInfo, args);

	public string Regex { get; set; } = args is null ? regex : string.Format(regex, args);
}