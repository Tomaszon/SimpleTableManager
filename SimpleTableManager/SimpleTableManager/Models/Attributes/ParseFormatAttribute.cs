namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ParseFormatAttribute : Attribute
{
	public string Format { get; set; }

	public string Regex { get; set; }

	public ParseFormatAttribute(string formatInfo, [StringSyntax(StringSyntaxAttribute.Regex)] string regex, object[]? args = null)
	{
		Format = args is null ? formatInfo : string.Format(formatInfo, args);

		if (args is not null)
		{
			Shared.IndexArray(args.Length).ForEach(i => regex = regex.Replace($"Arg{i}", args.ElementAt(i).ToString()));
		}

		Regex = regex;
	}
}