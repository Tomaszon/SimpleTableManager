using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

public partial class Size : IParsable<Size>
{
	[GeneratedRegex("\\d,\\d")]
	private static partial Regex GetParseRegex();

	public int Width { get; set; }

	public int Height { get; set; }

	[JsonConstructor]
	public Size(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public Size(Position position)
	{
		Width = position.X;
		Height = position.Y;
	}

	public override string ToString()
	{
		return $"W: {Width}, H: {Height}";
	}

	public void Deconstruct(out int w, out int h)
	{
		w = Width;
		h = Height;
	}

	[ParseFormat("width,height")]
	public static Size Parse(string value, IFormatProvider? _)
	{
		var values = value.Split(',');

		var w = int.Parse(values[0].Trim());
		var h = int.Parse(values[1].Trim());

		return new Size(w, h);
	}

	public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? _, [MaybeNullWhen(false)] out Size size)
	{
		if (value is null || !GetParseRegex().IsMatch(value))
		{
			size = null;

			return false;
		}

		try
		{
			size = Parse(value, null);

			return true;
		}
		catch
		{
			size = null;

			return false;
		}
	}
}