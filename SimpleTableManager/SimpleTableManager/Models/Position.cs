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

		[ParseFormat("x,y")]
		public static Position Parse(string value)
		{
			var values = value.Split(',');

			var x = int.Parse(values[0].Trim());
			var y = int.Parse(values[1].Trim());

			return new Position(x, y);
		}
	}
}
