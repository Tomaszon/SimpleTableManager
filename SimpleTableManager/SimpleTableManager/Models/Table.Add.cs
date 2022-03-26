﻿using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void AddRowAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Height, $"Index is not in the needed range: [0, {Size.Height}]");

			for (int x = 0; x < Size.Width; x++)
			{
				Cells.Insert(index * Size.Width + x, new Cell($"N:{x},{index}") { BackgroundColor = Settings.DefaultBackgroundColor, ForegroundColor = Settings.DefaultForegroundColor });
			}

			Size.Height++;
		}

		[CommandReference]
		public void AddRowAfter(int after)
		{
			Shared.Validate(() => after >= 0 && after <= Size.Height, $"Index is not in the needed range: [0, {Size.Height - 1}]");

			AddRowAt(after + 1);
		}

		[CommandReference]
		public void AddRowFirst()
		{
			AddRowAt(0);
		}

		[CommandReference]
		public void AddRowLast()
		{
			AddRowAt(Size.Height);
		}

		[CommandReference]
		public void AddColumnAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Width, $"Index is not in the needed range: [0, {Size.Width}]");

			for (int y = 0; y < Size.Height; y++)
			{
				Cells.Insert(Size.Width * y + y + index, new Cell($"NEW,y:{y}") { BackgroundColor = Settings.DefaultBackgroundColor, ForegroundColor = Settings.DefaultForegroundColor });
			}

			Size.Width++;
		}

		[CommandReference]
		public void AddColumnAfter(int after)
		{
			Shared.Validate(() => after >= 0 && after <= Size.Width, $"Index is not in the needed range: [0, {Size.Width - 1}]");

			AddColumnAt(after + 1);
		}

		[CommandReference]
		public void AddColumnFirst()
		{
			AddColumnAt(0);
		}

		[CommandReference]
		public void AddColumnLast()
		{
			AddColumnAt(Size.Width);
		}
	}
}
