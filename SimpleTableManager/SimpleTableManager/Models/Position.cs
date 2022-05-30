
using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class Position
	{
		public int X { get; set; }

		public int Y { get; set; }

		[JsonConstructor]
		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Position(Size size)
		{
			X = size.Width;
			Y = size.Height;
		}

		public override string ToString()
		{
			return $"X: {X}, Y: {Y}";
		}

		public void Deconstruct(out int x, out int y)
		{
			x = X;
			y = Y;
		}
	}
}
