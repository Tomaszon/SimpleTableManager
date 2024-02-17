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

	public string Format(string? _, object? arg, IFormatProvider? formatProvider)
	{
		if (string.IsNullOrEmpty(_format) || _format.Equals("null", StringComparison.OrdinalIgnoreCase))
		{
			return arg!.ToString()!;
		}

		if (arg is IFormattable p)
		{
			if (arg is decimal d)
			{
				return _format switch
				{
					"%" => $"{d * 100:0}%",
					//TODO
					// "diagram" => string.Join("\r\n", Enumerable.Repeat('x', (int)decimal.Round(Math.Max(Math.Min(d, 1), 0) * 5))),

					_ => p.ToString(_format, null)
				};
			}

			return p.ToString(_format, null);
		}
		else if (arg is bool b)
		{
			return _format switch
			{
				"YN" => b ? "Y" : "N",
				"YesNo" => b ? "Yes" : "No",

				_ => throw new FormatException()
			};
		}

		return arg!.ToString()!;
	}
}