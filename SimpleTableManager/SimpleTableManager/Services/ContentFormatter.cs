using SimpleTableManager.Models;

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
					"chart5v" => GetChart(d, 5),
					"chart10v" => GetChart(d, 10),
					"chart5h" => GetChart(d, 5, true),
					"chart5hl" => GetChart(d, 5, true, true),
					"chart10h" => GetChart(d, 10, true),
					"chart10hl" => GetChart(d, 10, true, true),

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
		else if (arg is IShape2Sized shape2Sized)
		{
			return $"S1: {shape2Sized.Size1.ToString(_format)}, S2: {shape2Sized.Size2.ToString(_format)}";
		}
		else if (arg is IShape1Sized shape1Sized)
		{
			return $"S1: {shape1Sized.Size1.ToString(_format)}";
		}

		return arg!.ToString()!;
	}

	private static string GetChart(decimal value, int size, bool horizontal = false, bool label = false)
	{
		var f = (int)decimal.Round(Math.Max(Math.Min(value, 1), 0) * size);
		var e = size - f;

		if (horizontal)
		{
			return
				(label ? $"{value:0.00} " : "") +
				string.Join("", Enumerable.Repeat('░', e)) +
				string.Join("", Enumerable.Repeat('█', f));
		}
		else
		{
			return
				string.Join("\r\n", Enumerable.Repeat('░', e)) +
				"\r\n" +
				string.Join("\r\n", Enumerable.Repeat('█', f));
		}
	}
}