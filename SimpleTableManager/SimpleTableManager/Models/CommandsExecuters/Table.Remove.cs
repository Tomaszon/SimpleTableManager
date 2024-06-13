﻿namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void RemoveRowAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Height - 1, $"Index is not in the needed range: [0, {Size.Height - 1}]");
		ThrowIfNot(Size.Height > 1, "Can not decrease table height under 1 row!");

		Content.RemoveRange(index * Size.Width, Size.Width);
		Sider.RemoveAt(index);

		Size.Height--;

		if (ViewOptions.EndPosition.Y == Size.Height)
		{
			ViewOptions.DecreaseHeight();
		}
	}

	[CommandFunction]
	public void RemoveFirstRow()
	{
		RemoveRowAt(0);
	}

	[CommandFunction]
	public void RemoveLastRow()
	{
		RemoveRowAt(Size.Height - 1);
	}

	[CommandFunction]
	public void RemoveColumnAt(int index)
	{
		ThrowIfNot(index >= 0 && index <= Size.Width - 1, $"Index is not in the needed range: [0, {Size.Width - 1}]");
		ThrowIfNot(Size.Width > 1, "Can not decrease table width under 1 column!");

		for (int y = 0; y < Size.Height; y++)
		{
			Content.RemoveAt(Size.Width * y - y + index);
		}
		Header.RemoveAt(index);

		Size.Width--;

		if (ViewOptions.EndPosition.X == Size.Width)
		{
			ViewOptions.DecreaseWidth();
		}
	}

	[CommandFunction]
	public void RemoveFirstColumn()
	{
		RemoveColumnAt(0);
	}

	[CommandFunction]
	public void RemoveLastColumn()
	{
		RemoveColumnAt(Size.Width - 1);
	}
}