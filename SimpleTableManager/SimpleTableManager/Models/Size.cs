using System.Text.RegularExpressions;

namespace SimpleTableManager.Models;

[ParseFormat("width,height", "(?<w>\\d+),(?<h>\\d+)"), ParseFormat("width;height", "(?<w>\\d+);(?<h>\\d+)")]
public class Size : ParsableBase<Size>, IParsable<Size>, IParseCore<Size>
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
		return $"W:{Width}, H:{Height}";
	}

	public void Deconstruct(out int w, out int h)
	{
		w = Width;
		h = Height;
	}

	public static Size ParseCore(GroupCollection args)
	{
		var w = int.Parse(args["w"].Value);
		var h = int.Parse(args["h"].Value);

		return new Size(w, h);
	}
}