namespace SimpleTableManager.Models
{
	public class ViewOptions
	{
		public Position StartPosition { get; set; }

		public Position EndPosition { get; set; }

		[JsonIgnore]
		public Size Size => new(EndPosition.X - StartPosition.X + 1, EndPosition.Y - StartPosition.Y + 1);

		public ViewOptions(int x1, int y1, int x2, int y2)
		{
			StartPosition = new Position(x1, y1);
			EndPosition = new Position(x2, y2);
		}

		public void IncreaseWidth()
		{
			EndPosition.X++;
		}

		public void DecreaseWidth()
		{
			if (EndPosition.X < 1)
			{
				throw new InvalidOperationException("Can not decrease view width under 1 column!\n    Increase window width!");
			}

			EndPosition.X = EndPosition.X - 1;
		}

		public void IncreaseHeight()
		{
			EndPosition.Y++;
		}

		public void DecreaseHeight()
		{
			if (EndPosition.Y < 1)
			{
				throw new InvalidOperationException("Can not decrease view height under 1 row!\n    Increase window heigh!");
			}

			EndPosition.Y = EndPosition.Y - 1;
		}
	}
}
