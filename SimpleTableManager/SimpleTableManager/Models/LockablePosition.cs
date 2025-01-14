namespace SimpleTableManager.Models;

public class LockablePosition(int x, int y, bool horizontallyLocked = true, bool verticallyLocked = true) : Position(x, y)
{
	public bool IsHorizontallyLocked { get; set; } = horizontallyLocked;

	public bool IsVerticallyLocked { get; set; } = verticallyLocked;

	public void Shift(Size size)
	{
		if (!IsHorizontallyLocked)
		{
			X += size.Width;
		}

		if (!IsVerticallyLocked)
		{
			Y += size.Height;
		}
	}

	public override string ToString()
	{
		return $"X:{(IsHorizontallyLocked ? "" : Shared.REF_CHAR)}{X},Y:{(IsVerticallyLocked ? "" : Shared.REF_CHAR)}{Y}";
	}

	public string ToShortString()
	{
		return $"{(IsHorizontallyLocked ? "" : Shared.REF_CHAR)}{X}:{(IsVerticallyLocked ? "" : Shared.REF_CHAR)}{Y}";
	}
}
