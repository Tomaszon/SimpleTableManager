using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

[ParseFormat("width,height", "\\d,\\d"), ParseFormat("width;height", "\\d;\\d")]
public class Size : ParsableBase<Size>, IParsable<Size>
{
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

	public static Size Parse(string value, IFormatProvider? _)
	{
		return ParseWrapper(value, args =>
		{
			var w = int.Parse(args[0].Trim());
			var h = int.Parse(args[1].Trim());

			return new Size(w, h);
		});
	}
}