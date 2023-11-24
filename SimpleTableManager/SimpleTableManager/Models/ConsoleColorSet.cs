namespace SimpleTableManager.Models;

public class ConsoleColorSet
{
	public ConsoleColor Foreground { get; set; } = ConsoleColor.Gray;

	public ConsoleColor Background { get; set; } = ConsoleColor.Black;

	[JsonConstructor]
	public ConsoleColorSet(ConsoleColor? foreground = null, ConsoleColor? background = null)
	{
		if (foreground is not null)
		{
			Foreground = foreground.Value;
		}
		if (background is not null)
		{
			Background = background.Value;
		}
	}

	public ConsoleColorSet(ConsoleColorSet colorSet)
	{
		Foreground = colorSet.Foreground;
		Background = colorSet.Background;
	}

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
		return $"F: {Foreground}, B: {Background}";
	}
}