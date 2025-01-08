namespace SimpleTableManager.Models;

[ParseFormat("width*height", "^(?<w>\\d+)\\*(?<h>\\d+)$")]
[ParseFormat("widthxheight", "^(?<w>\\d+)x(?<h>\\d+)$")]
[method: JsonConstructor]
public class Size(int width, int height) : ParsableBase<Size>, IParsable<Size>, IParseCore<Size>
{
	public int Width { get; set; } = width;

	public int Height { get; set; } = height;

	public Size(Position position) : this(position.X, position.Y) { }

	public override string ToString()
	{
		return $"W:{Width}, H:{Height}";
	}

	public void Deconstruct(out int w, out int h)
	{
		w = Width;
		h = Height;
	}

	public static Size ParseCore(GroupCollection args, IFormatProvider? _)
	{
		var w = int.Parse(args["w"].Value);
		var h = int.Parse(args["h"].Value);

		return new Size(w, h);
	}
}