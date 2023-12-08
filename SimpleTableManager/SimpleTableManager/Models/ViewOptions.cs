namespace SimpleTableManager.Models;

public class ViewOptions : ValidatorBase
{
	public event Action? ViewChanged;

	public Position StartPosition { get; set; }

	public Position EndPosition { get; set; }

	[JsonIgnore]
	public Size Size => new(EndPosition.X - StartPosition.X + 1, EndPosition.Y - StartPosition.Y + 1);

	public ViewOptions(int x1, int y1, int x2, int y2)
	{
		Set(x1, y1, x2, y2);
	}

	[MemberNotNull(nameof(StartPosition)), MemberNotNull(nameof(EndPosition))]
	public void Set(int x1, int y1, int x2, int y2)
	{
		StartPosition = new(x1, y1);
		EndPosition = new(x2, y2);

		ViewChanged?.Invoke();
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
		ThrowIf<InvalidOperationException>(EndPosition.X <= StartPosition.X, "Can not decrease view width under 0 columns!");
		
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
		ThrowIf<InvalidOperationException>(EndPosition.Y <= StartPosition.Y, "Can not decrease view height under 0 rows!");

		EndPosition.Y = EndPosition.Y - 1;

		ViewChanged?.Invoke();
	}
}