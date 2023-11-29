namespace SimpleTableManager.Services;

public class ContentFormatter : IFormatProvider, ICustomFormatter
{
	private readonly string? _format;

	public ContentFormatter(string? format)
	{
		_format = format;
	}

	public object? GetFormat(Type? formatType)
	{
		return this;
	}

	public string Format(string? format, object? arg, IFormatProvider? formatProvider)
	{
		if (arg is IFormattable p)
		{
			return p.ToString(_format, null);
		}
		else if (arg is bool b)
		{
			return _format switch
			{
				"YN" => b ? "Y" : "N",
				"YesNo" => b ? "Yes" : "No",
				"" => b.ToString(),
				null => b.ToString(),

				_ => throw new FormatException()
			};
		}

		return arg!.ToString()!;
	}
}