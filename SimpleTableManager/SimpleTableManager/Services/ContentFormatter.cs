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
		if (string.IsNullOrEmpty(_format) || _format.Equals("null", StringComparison.OrdinalIgnoreCase))
		{
			return arg!.ToString()!;
		}

		if (arg is IFormattable p)
		{
			if (arg is double d)
			{
				return _format switch
				{
					"%" => $"{d * 100:0}%",
					//HACK
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
			return _format.ToLower() switch
			{
				"yn" => b ? "Y" : "N",
				"yesno" => b ? "Yes" : "No",

				_ => b.ToString()
			};
		}

		return arg!.ToString()!;
	}

	private static string GetChart(double value, int size, bool horizontal = false, bool label = false)
	{
		var f = (int)double.Round(Math.Max(Math.Min(value, 1), 0) * size);
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
				string.Join("\n", Enumerable.Repeat('░', e)) +
				"\n" +
				string.Join("\n", Enumerable.Repeat('█', f));
		}
	}
}