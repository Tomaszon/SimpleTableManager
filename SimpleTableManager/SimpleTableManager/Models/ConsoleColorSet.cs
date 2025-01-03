using System.Numerics;

namespace SimpleTableManager.Models;

[method: JsonConstructor]
public readonly struct ConsoleColorSet(ConsoleColor? foreground, ConsoleColor? background) : IEqualityOperators<ConsoleColorSet, ConsoleColorSet, bool>
{
	public ConsoleColor Foreground { get; } = foreground ?? ConsoleColor.Gray;

	public ConsoleColor Background { get; } = background ?? ConsoleColor.Black;

	public ConsoleColorSet(ConsoleColorSet colorSet) : this(colorSet.Foreground, colorSet.Background) { }

	public static implicit operator ConsoleColorSet((ConsoleColor? foreground, ConsoleColor? background) record)
	{
		return new ConsoleColorSet(record.foreground, record.background);
	}

	public static bool operator ==(ConsoleColorSet left, ConsoleColorSet right)
	{
		return left.Foreground == right.Foreground && left.Background == right.Background;
	}

	public static bool operator !=(ConsoleColorSet left, ConsoleColorSet right)
	{
		return !(left == right);
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		if (obj is ConsoleColorSet another)
		{
			return this == another;
		}

		return false;
	}

	public override readonly int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override readonly string ToString()
	{
		return $"F:{Foreground}, B:{Background}";
	}
}