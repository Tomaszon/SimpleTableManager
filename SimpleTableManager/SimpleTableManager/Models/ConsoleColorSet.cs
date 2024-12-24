namespace SimpleTableManager.Models;

[method: JsonConstructor]
public class ConsoleColorSet(ConsoleColor? foreground, ConsoleColor? background)
{
	public ConsoleColor Foreground { get; set; } = foreground ?? ConsoleColor.Gray;

	public ConsoleColor Background { get; set; } = background ?? ConsoleColor.Black;

	public ConsoleColorSet(ConsoleColorSet colorSet) : this(colorSet.Foreground, colorSet.Background) { }

	public static implicit operator ConsoleColorSet((ConsoleColor? foreground, ConsoleColor? background) record)
	{
		return new ConsoleColorSet(record.foreground, record.background);
	}

	public override bool Equals(object? obj)
	{
		if (obj is ConsoleColorSet another && another is not null)
		{
			return Foreground == another.Foreground && Background == another.Background;
		}

		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return $"F:{Foreground}, B:{Background}";
	}
}