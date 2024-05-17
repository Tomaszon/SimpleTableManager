using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

public abstract class ParsableBase<T>
where T : class, IParsable<T>
{
	public static T ParseWrapper(string value, Func<string[], T> func)
	{
		var regexes = typeof(T).GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Regex);

		foreach (var regex in regexes)
		{
			if (Regex.IsMatch(value, regex))
			{
				var values = Regex.Split(value, ",|;");

				return func(values);
			}
		}

		throw new FormatException();
	}

	public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? _, [NotNullWhen(true)] out T? result)
	{
		if (value is null)
		{
			result = null;

			return false;
		}

		try
		{
			result = T.Parse(value, null);

			return true;
		}
		catch
		{
			result = null;

			return false;
		}
	}
}