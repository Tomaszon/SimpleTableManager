namespace SimpleTableManager.Services;

public class ContentFormatter(string? format) : IFormatProvider, ICustomFormatter
{
	private readonly string? _format = format;

	public object? GetFormat(Type? formatType)
	{
		return this;
	}

	public string Format(string? _, object? arg, IFormatProvider? formatProvider)
	{
		if (string.IsNullOrEmpty(_format) ||
			_format.Equals("null", StringComparison.OrdinalIgnoreCase) ||
			arg is not IFormattable p)
		{
			return arg!.ToString()!;
		}
			
		return p.ToString(_format, null);
	}
}