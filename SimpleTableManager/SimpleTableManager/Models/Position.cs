using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

public partial class Position : IParsable<Position>
{
	[GeneratedRegex("\\d,\\d")]
	private static partial Regex GetParseRegex();

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

	[ParseFormat("x,y")]
	public static Position Parse(string value, IFormatProvider? _)
	{
		var values = value.Split(',');

		var x = int.Parse(values[0].Trim());
		var y = int.Parse(values[1].Trim());

		return new Position(x, y);
	}

	public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? _, [MaybeNullWhen(false)] out Position position)
	{
		if (value is null || !GetParseRegex().IsMatch(value))
		{
			position = null;

			return false;
		}

		try
		{
			position = Parse(value, null);

			return true;
		}
		catch
		{
			position = null;

			return false;
		}
	}
}