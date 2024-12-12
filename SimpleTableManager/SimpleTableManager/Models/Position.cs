using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

[ParseFormat("x,y", "(?<x>\\d),(?<y>\\d)"), ParseFormat("x;y", "(?<x>\\d);(?<y>\\d)")]
public class Position : ParsableBase<Position>, IParsable<Position>, IParseCore<Position>
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
		return $"X:{X}, Y:{Y}";
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

	public static Position ParseCore(GroupCollection args)
	{
		var x = int.Parse(args["x"].Value);
		var y = int.Parse(args["y"].Value);

		return new Position(x, y);
	}
}