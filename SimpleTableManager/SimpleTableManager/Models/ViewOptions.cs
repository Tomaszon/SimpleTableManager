namespace SimpleTableManager.Models;

public class ViewOptions : ValidatorBase
{
	public event Action? ViewChanged;

	public Position StartPosition { get; set; }

	public Position EndPosition { get; set; }

	public Size Size => new(EndPosition.X - StartPosition.X + 1, EndPosition.Y - StartPosition.Y + 1);

	public ViewOptions(int x1, int y1, int x2, int y2)
	{
		Set(x1, y1, x2, y2, false);
	}

	[MemberNotNull(nameof(StartPosition)), MemberNotNull(nameof(EndPosition))]
	public void Set(int x1, int y1, int x2, int y2, bool triggerEvent = true)
	{
		StartPosition = new(x1, y1);
		EndPosition = new(x2, y2);

		if (triggerEvent)
		{
			ViewChanged?.Invoke();
		}
	}

	public void InvokeViewChangedEvent()
	{
		ViewChanged?.Invoke();
	}

	public void IncreaseWidth(bool triggerEvent = true)
	{
		EndPosition.X++;

		if (triggerEvent)
		{
			ViewChanged?.Invoke();
		}
	}

	public void DecreaseWidth(bool triggerEvent = true)
	{
		ThrowIf<InvalidOperationException>(EndPosition.X <= StartPosition.X, "Can not decrease view width under 0 columns!");

		EndPosition.X = EndPosition.X - 1;

		if (triggerEvent)
		{
			ViewChanged?.Invoke();
		}
	}

	public void IncreaseHeight(bool triggerEvent = true)
	{
		EndPosition.Y++;

		if (triggerEvent)
		{
			ViewChanged?.Invoke();
		}
	}

	public void DecreaseHeight(bool triggerEvent = true)
	{
		ThrowIf<InvalidOperationException>(EndPosition.Y <= StartPosition.Y, "Can not decrease view height under 0 rows!");

		EndPosition.Y = EndPosition.Y - 1;

		if (triggerEvent)
		{
			ViewChanged?.Invoke();
		}
	}
}