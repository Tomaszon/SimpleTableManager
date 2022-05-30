
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
	}
}
