namespace SimpleTableManager.Models;

public abstract class ParsableBase<T>
where T : class, IParsable<T>, IParseCore<T>
{
	public static T Parse(string value, IFormatProvider? formatProvider)
	{
		var match = GetRightMatch(value);

		if (match is not null)
		{
			return T.ParseCore(match.Groups, formatProvider);
		}

		throw new FormatException();
	}

	public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? formatProvider, [NotNullWhen(true)] out T? result)
	{
		var match = GetRightMatch(value);

		if (match is not null)
		{
			result = T.ParseCore(match.Groups, formatProvider);

			return true;
		}

		result = null;

		return false;
	}

	private static Match? GetRightMatch(string? value)
	{
		if (value is null)
		{
			return null;
		}

		var regexes = typeof(T).GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Regex);

		foreach (var regex in regexes)
		{
			var match = Regex.Match(value, regex);

			if (match.Success)
			{
				return match;
			}
		}

		return null;
	}
}