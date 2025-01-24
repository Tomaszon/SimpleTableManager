namespace SimpleTableManager.Models;

public abstract class ParsableBase<T>
	where T : class, IParsable<T>, IParseCore<T>
{
	public static T Parse(string value, IFormatProvider? formatProvider)
	{
		var match = GetRightMatch(value, out var formats);

		if (match is not null)
		{
			return T.ParseCore(match.Groups, formatProvider);
		}

		throw new FormatException($"Can not format value '{value}' to type '{typeof(T).GetFriendlyName()}'{(formats.Any() ? $" Required formats: '{string.Join("' '", formats)}'" : "")}");
	}

	public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? formatProvider, [NotNullWhen(true)] out T? result)
	{
		var match = GetRightMatch(value, out _);

		if (match is not null)
		{
			result = T.ParseCore(match.Groups, formatProvider);

			return true;
		}

		result = null;

		return false;
	}

	private static Match? GetRightMatch(string? value, out IEnumerable<string> formats)
	{
		var attributes = typeof(T).GetCustomAttributes<ParseFormatAttribute>();

		formats = attributes.Select(a => a.Format);

		if (value is null)
		{
			return null;
		}

		var regexes = attributes.Select(a => a.Regex);

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