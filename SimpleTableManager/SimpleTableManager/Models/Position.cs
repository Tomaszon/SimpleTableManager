﻿using System.Numerics;

namespace SimpleTableManager.Models;

[ParseFormat("x,y", "^(?<x>\\d+),(?<y>\\d+)$"), ParseFormat("x;y", "^(?<x>\\d+);(?<y>\\d+)$")]
[method: JsonConstructor]
public class Position(int x, int y) : ParsableBase<Position>, IParsable<Position>, IParseCore<Position>, ISubtractionOperators<Position, Position, Size>
{
	public int X { get; set; } = x;

	public int Y { get; set; } = y;

	public Position(Size size) : this(size.Width, size.Height) { }

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

	public static Position ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var x = int.Parse(args["x"].Value);
		var y = int.Parse(args["y"].Value);

		return new Position(x, y);
	}

	public static Size operator -(Position left, Position right)
	{
		return new(left.X - right.X, left.Y - right.Y);
	}
}