
using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class Size
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

		[ParseFormat("width,height")]
		public static Size Parse(string value)
		{
			var values = value.Split(',');

			var w = int.Parse(values[0].Trim());
			var h = int.Parse(values[1].Trim());

			return new Size(w, h);
		}
	}
}
