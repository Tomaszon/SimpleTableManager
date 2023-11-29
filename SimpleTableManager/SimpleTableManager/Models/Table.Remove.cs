using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

public partial class Table
{
	[CommandReference]
	public void RemoveRowAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Height - 1, $"Index is not in the needed range: [0, {Size.Height - 1}]");
		ThrowIfNot(Size.Height > 1, "Can not decrease table height under 1 row!");

		Content.RemoveRange(index * Size.Width, Size.Width);
		Sider.RemoveAt(index);

		if (ViewOptions.EndPosition.Y == Size.Height - 1)
		{
			ViewOptions.DecreaseHeight();
		}

		Size.Height--;
	}

	[CommandReference]
	public void RemoveFirstRow()
	{
		RemoveRowAt(0);
	}

	[CommandReference]
	public void RemoveLastRow()
	{
		RemoveRowAt(Size.Height - 1);
	}

	[CommandReference]
	public void RemoveColumnAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Width - 1, $"Index is not in the needed range: [0, {Size.Width - 1}]");
		ThrowIfNot(Size.Width > 1, "Can not decrease table width under 1 column!");

		for (int y = 0; y < Size.Height; y++)
		{
			Content.RemoveAt(Size.Width * y - y + index);
		}
		Header.RemoveAt(index);

		if (ViewOptions.EndPosition.X == Size.Width - 1)
		{
			ViewOptions.DecreaseWidth();
		}

		Size.Width--;
	}

	[CommandReference]
	public void RemoveFirstColumn()
	{
		RemoveColumnAt(0);
	}

	[CommandReference]
	public void RemoveLastColumn()
	{
		RemoveColumnAt(Size.Width - 1);
	}
}