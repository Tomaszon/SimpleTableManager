using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

[ParseFormat("x,y", "\\d,\\d"), ParseFormat("x;y", "\\d;\\d")]
public class Position : ParsableBase<Position>, IParsable<Position>
{
	public int X { get; set; }

	public int Y { get; set; }

	[JsonConstructor]
	public Position(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Position(Size size)
	{
		X = size.Width;
		Y = size.Height;
	}

	public override string ToString()
	{
		return $"X: {X}, Y: {Y}";
	}

	public override bool Equals(object? obj)
	{
		if (obj is Position p && p is not null)
		{
			return X == p.X && Y == p.Y;
		}

		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public void Deconstruct(out int x, out int y)
	{
		x = X;
		y = Y;
	}

	public new static Position Parse(string value, IFormatProvider? _)
	{
		return ParseWrapper(value, (args) =>
		{
			var x = int.Parse(args[0].Trim());
			var y = int.Parse(args[1].Trim());

			return new Position(x, y);
		});
	}

	// public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? _, [NotNullWhen(true)] out Position? position)
	// {

	// 	throw new NotImplementedException();
	// }

	// public static Position Parse(string value, IFormatProvider? _)
	// {
	// 	var regexes = typeof(Position).GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Regex);

	// 	foreach (var regex in regexes)
	// 	{
	// 		if (Regex.IsMatch(value, regex))
	// 		{
	// 			var values = Regex.Split(value, ",|;");

	// 			var x = int.Parse(values[0].Trim());
	// 			var y = int.Parse(values[1].Trim());

	// 			return new Position(x, y);
	// 		}
	// 	}

	// 	throw new FormatException();
	// }

	// public override Func<string[], Position> ParseCore =>
	// 	new((args) =>
	// 	{
	// 		var x = int.Parse(args[0].Trim());
	// 		var y = int.Parse(args[1].Trim());

	// 		return new Position(x, y);
	// 	});


	// public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? _, [NotNullWhen(true)] out Position? position)
	// {
	// 	if (value is null)
	// 	{
	// 		position = null;

	// 		return false;
	// 	}

	// 	try
	// 	{
	// 		position = Parse(value, null);

	// 		return true;
	// 	}
	// 	catch
	// 	{
	// 		position = null;

	// 		return false;
	// 	}
	// }

}

public abstract class ParsableBase<T> : IParsable<T>
where T : class, IParsable<T>
{
    public static T Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static T ParseWrapper(string value, Func<string[], T> func)
	{
		var regexes = typeof(Position).GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Regex);

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

	// public static bool TryParseWrapper(string value, Func<string>)
	// {
	// 	if (value is null)
	// 	{
	// 		result = null;

	// 		return false;
	// 	}

	// 	try
	// 	{
	// 		result = T.Parse(value, null);

	// 		return true;
	// 	}
	// 	catch
	// 	{
	// 		result = null;

	// 		return false;
	// 	}
	// }

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