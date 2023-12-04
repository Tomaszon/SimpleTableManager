namespace SimpleTableManager.Models;

public class ViewOptions
{
	public event Action? ViewChanged;

	public Position StartPosition { get; set; }

	public Position EndPosition { get; set; }

	[JsonIgnore]
	public Size Size => new(EndPosition.X - StartPosition.X + 1, EndPosition.Y - StartPosition.Y + 1);

	public ViewOptions(int x1, int y1, int x2, int y2)
	{
		StartPosition = new Position(x1, y1);
		EndPosition = new Position(x2, y2);
	}

	public void InvokeViewChangedEvent()
	{
		ViewChanged?.Invoke();
	}

	public void IncreaseWidth()
	{
		EndPosition.X++;

		ViewChanged?.Invoke();
	}

	public void DecreaseWidth()
	{
		EndPosition.X = EndPosition.X - 1;

		ViewChanged?.Invoke();
	}

	public void IncreaseHeight()
	{
		EndPosition.Y++;

		ViewChanged?.Invoke();
	}

	public void DecreaseHeight()
	{
		EndPosition.Y = EndPosition.Y - 1;

		ViewChanged?.Invoke();
	}
}