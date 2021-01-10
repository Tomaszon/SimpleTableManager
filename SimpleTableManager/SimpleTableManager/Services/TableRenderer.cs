using System;
using System.Collections.Generic;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class TableRenderer
	{
		public static void Render(Table table, ViewOptions viewOptions = null)
		{
			Size cellSize = new Size(7, 1);

			viewOptions ??= new ViewOptions(0, 0, table.Size.Width, table.Size.Height);

			DrawHeaderRow(viewOptions, cellSize);

			for (int y = viewOptions.Position.Y; y < viewOptions.Position.Y + viewOptions.Size.Height - 1; y++)
			{
				DrawInnerRow(y, cellSize, table[viewOptions.Position.X, y, viewOptions.Position.X + viewOptions.Size.Width - 1, y]);
			}

			int lastRowIndex = viewOptions.Position.Y + viewOptions.Size.Height - 1;

			DrawLastRow(lastRowIndex, cellSize, table[viewOptions.Position.X, lastRowIndex, viewOptions.Position.X + viewOptions.Size.Width - 1, lastRowIndex]);
		}

		private static void DrawHeaderRow(ViewOptions viewOptions, Size cellSize)
		{
			DrawFirstHorizontalBorder(cellSize, viewOptions);

			DrawHeaderContentLine(cellSize, viewOptions);

			DrawHeaderSeparatorBorder(cellSize, viewOptions);
		}

		private static void DrawInnerRow(int row, Size cellSize, List<Cell> cells)
		{
			DrawContentLine(row, cellSize, cells);

			DrawInnerHorizontalBorder(cellSize, cells);
		}

		private static void DrawLastRow(int row, Size cellSize, List<Cell> cells)
		{
			DrawContentLine(row, cellSize, cells);

			DrawLastHorizontalBorder(cellSize, cells);
		}

		private static void DrawFirstHorizontalBorder(Size cellSize, ViewOptions viewOptions)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleRight_DoubleDown), DrawColorSet.Border);
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight_DoubleDown), DrawColorSet.Border);
			for (int i = 0; i < viewOptions.Size.Width - 1; i++)
			{
				Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight), cellSize.Width), DrawColorSet.Border);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight_SingleDown), DrawColorSet.Border);
			}
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_SingleDown), DrawColorSet.Border);
			Console.WriteLine();
		}

		private static void DrawHeaderContentLine(Size cellSize, ViewOptions viewOptions)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_DoubleDown), DrawColorSet.Border);
			Draw(@"y \ x".PadLeftRight(cellSize.Width), DrawColorSet.Default);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_DoubleDown), DrawColorSet.Border);
			for (int i = viewOptions.Position.X; i < viewOptions.Position.X + viewOptions.Size.Width; i++)
			{
				Draw($"{i + 1}".PadLeftRight(cellSize.Width), DrawColorSet.Default);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_SingleDown), DrawColorSet.Border);
			}
			Console.WriteLine();
		}

		private static void DrawHeaderSeparatorBorder(Size cellSize, ViewOptions viewOptions)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_DoubleRight_DoubleDown), DrawColorSet.Border);
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_DoubleLeft_DoubleRight_DoubleDown), DrawColorSet.Border);
			for (int i = 0; i < viewOptions.Size.Width - 1; i++)
			{
				Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight), cellSize.Width), DrawColorSet.Border);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_DoubleLeft_DoubleRight_SingleDown), DrawColorSet.Border);
			}
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.DoubleLeft_DoubleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_DoubleLeft_SingleDown), DrawColorSet.Border);
			Console.WriteLine();
		}

		private static void DrawContentLine(int row, Size cellSize, List<Cell> cells)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_DoubleDown), DrawColorSet.Border);
			Draw($"{row + 1}".PadLeftRight(cellSize.Width), DrawColorSet.Default);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_DoubleDown), DrawColorSet.Border);
			foreach (var cell in cells)
			{
				Draw(cell);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_SingleDown), DrawColorSet.Border);
			}
			Console.WriteLine();
		}

		private static void DrawInnerHorizontalBorder(Size cellSize, List<Cell> cells)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_SingleRight_DoubleDown), DrawColorSet.Border);
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.SingleLeft_SingleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_SingleLeft_SingleRight_DoubleDown), DrawColorSet.Border);
			for (int i = 0; i < cells.Count - 1; i++)
			{
				Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.SingleLeft_SingleRight), cellSize.Width), DrawColorSet.Border);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_SingleLeft_SingleRight_SingleDown), DrawColorSet.Border);
			}
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.SingleLeft_SingleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_SingleLeft_SingleDown), DrawColorSet.Border);
			Console.WriteLine();
		}

		private static void DrawLastHorizontalBorder(Size cellSize, List<Cell> cells)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_SingleRight), DrawColorSet.Border);
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.SingleLeft_SingleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DoubleUp_SingleLeft_SingleRight), DrawColorSet.Border);
			for (int i = 0; i < cells.Count - 1; i++)
			{
				Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.SingleLeft_SingleRight), cellSize.Width), DrawColorSet.Border);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_SingleLeft_SingleRight), DrawColorSet.Border);
			}
			Draw(new string(TableBorderCharacters.Get(TableBorderCharacter.SingleLeft_SingleRight), cellSize.Width), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.SingleUp_SingleLeft), DrawColorSet.Border);
			Console.WriteLine();

			ChangeToDefaultColors();
		}

		private static void Draw(Cell cell)
		{
			Console.BackgroundColor = cell.BackgroundColor;
			Console.ForegroundColor = cell.ForegroundColor;

			Console.Write(cell.Content.ToString().PadLeftRight(cell.Size.Width));

			ChangeToDefaultColors();
		}

		private static void Draw(object content, DrawColorSet colorSet)
		{
			switch (colorSet)
			{
				case DrawColorSet.Default:
					ChangeToDefaultColors();
					break;

				case DrawColorSet.Border:
					ChangeToBorderColors();
					break;
			}

			Console.Write(content);
		}

		private static void ChangeToDefaultColors()
		{
			Console.ForegroundColor = Settings.DefaultForegroundColor;
			Console.BackgroundColor = Settings.DefaultBackgroundColor;
		}

		private static void ChangeToBorderColors()
		{
			Console.ForegroundColor = Settings.BorderColor;
			Console.BackgroundColor = Settings.DefaultBackgroundColor;
		}

		private enum DrawColorSet
		{
			Default,
			Border
		}
	}
}
