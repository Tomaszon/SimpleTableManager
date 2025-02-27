using System.Numerics;

namespace SimpleTableManager.Models;

[ParseFormat("x,y", "^(?<x>\\d+),(?<y>\\d+)$")]
[method: JsonConstructor]
public class Position(int x, int y) : ParsableBase<Position>, IParsable<Position>, IParsableCore<Position>, ISubtractionOperators<Position, Position, Size>, IAdditionOperators<Position, Size, Position>
{
	public int X { get; set; } = x;

	public int Y { get; set; } = y;

	public Position(Size size) :
		this(size.Width, size.Height)
	{ }

	public override string ToString()
	{
		return $"X:{X},Y:{Y}";
	}

	public override bool Equals(object? obj)
	{
		if (obj is Position p && p is not null)
		{
			return X == p.X && Y == p.Y;
		}

		return false;
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

	public static Position operator +(Position left, Size right)
	{
		return new Position(left.X + right.Width, left.Y + right.Height);
	}

	public bool IsBetween(Position from, Position to)
	{
		return Y >= from.Y && Y <= to.Y && X >= from.X && X <= to.X;
	}

	public bool IsNotBetween(Position from, Position to)
	{
		return !IsBetween(from, to);
	}

	public static List<Position> Range(Position from, Position to)
	{
		var results = new List<Position>();

		var startPosition = new Position(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
		var endPosition = new Position(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y));

		var position = new Position(startPosition.X, startPosition.Y);

		while (!position.Equals(endPosition))
		{
			results.Add(position);

			var offset = new Size(0, 0);

			if (position.X < endPosition.X)
			{
				offset.Width = 1;
			}
			else if (position.Y < endPosition.Y)
			{
				offset.Width = startPosition.X - position.X;
				offset.Height = 1;
			}

			position += offset;
		}

		results.Add(position);

		return results;
	}
}