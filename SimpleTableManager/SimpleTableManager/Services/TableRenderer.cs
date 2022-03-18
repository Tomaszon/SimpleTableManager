using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class TableRenderer
	{
		private static List<int> _COLUMN_WIDTHS;

		public static void Render(Table table, ViewOptions viewOptions = null)
		{
			viewOptions ??= new ViewOptions(0, 0, table.Size.Width, table.Size.Height);

			var viewTable = CreateViewTable(table, viewOptions);

			_COLUMN_WIDTHS = new List<int>();
			for (int i = 0; i < viewTable.Size.Width; i++)
			{
				_COLUMN_WIDTHS.Add(viewTable.GetColumnWidth(i));
			}

			DrawHeaderRow(viewTable);

			for (int y = 1; y < viewTable.Size.Height; y++)
			{
				DrawContentRow(viewTable, y);
			}

			ChangeToDefaultColors();
		}

		private static void DrawHeaderRow(Table viewTable)
		{
			DrawSeparatorLine(TableBorderCharacter.DR_DD, TableBorderCharacter.DL_DR_DD, TableBorderCharacter.DL_DR_SD, TableBorderCharacter.DL_SD, TableBorderCharacter.DL_DR, viewTable.Size.Width);

			DrawContentLine(viewTable[0, 0, viewTable.Size.Width - 1, 0]);

			DrawSeparatorLine(TableBorderCharacter.DU_DR_DD, TableBorderCharacter.DU_DL_DR_DD, TableBorderCharacter.SU_DL_DR_SD, TableBorderCharacter.SU_DL_SD, TableBorderCharacter.DL_DR, viewTable.Size.Width);
		}

		private static void DrawContentRow(Table viewTable, int index)
		{
			DrawContentLine(viewTable[0, index, viewTable.Size.Width - 1, index]);

			if (index == viewTable.Size.Height - 1)
			{
				DrawSeparatorLine(TableBorderCharacter.DU_SR, TableBorderCharacter.DU_SL_SR, TableBorderCharacter.SU_SL_SR, TableBorderCharacter.SU_SL, TableBorderCharacter.SL_SR, viewTable.Size.Width);
			}
			else
			{
				DrawSeparatorLine(TableBorderCharacter.DU_SR_DD, TableBorderCharacter.DU_SL_SR_DD, TableBorderCharacter.SU_SL_SR_SD, TableBorderCharacter.SU_SL_SD, TableBorderCharacter.SL_SR, viewTable.Size.Width);
			}
		}

		private static void DrawSeparatorLine(TableBorderCharacter node1, TableBorderCharacter node2, TableBorderCharacter node3, TableBorderCharacter node4, TableBorderCharacter normal, int columnCount)
		{
			Draw(TableBorderCharacters.Get(node1), DrawColorSet.Border);
			Draw(new string(TableBorderCharacters.Get(normal), _COLUMN_WIDTHS[0]), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(node2), DrawColorSet.Border);

			for (int i = 1; i < columnCount - 1; i++)
			{
				Draw(new string(TableBorderCharacters.Get(normal), _COLUMN_WIDTHS[i]), DrawColorSet.Border);
				Draw(TableBorderCharacters.Get(node3), DrawColorSet.Border);
			}

			Draw(new string(TableBorderCharacters.Get(normal), _COLUMN_WIDTHS.Last()), DrawColorSet.Border);
			Draw(TableBorderCharacters.Get(node4), DrawColorSet.Border);
			Console.WriteLine();
		}

		private static void DrawContentLine(List<Cell> cells)
		{
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DU_DD), DrawColorSet.Border);
			Draw(cells[0], _COLUMN_WIDTHS[0]);
			Draw(TableBorderCharacters.Get(TableBorderCharacter.DU_DD), DrawColorSet.Border);

			for (int i = 1; i < cells.Count; i++)
			{
				Draw(cells[i], _COLUMN_WIDTHS[i]);
				Draw(TableBorderCharacters.Get(TableBorderCharacter.SU_SD), DrawColorSet.Border);
			}
			Console.WriteLine();
		}

		private static void Draw(Cell cell, int width)
		{
			if (cell.IsSelected)
			{
				ChangeToSelectedColor();
			}
			else
			{
				ChangeToDefaultColors();
			}

			Console.Write(cell.Content.ToString().PadLeftRight(width));

			ChangeToDefaultColors();
		}

		private static void Draw(object content, int width = 1, DrawColorSet colorSet = DrawColorSet.Default)
		{
			switch (colorSet)
			{
				case DrawColorSet.Default:
					ChangeToDefaultColors();
					break;

				case DrawColorSet.Border:
					ChangeToBorderColors();
					break;
				case DrawColorSet.SelectedBorder:
					ChangeToSelectedColor();
					break;
			}

			Console.Write(content.ToString().PadLeftRight(width));
		}

		private static void Draw(object content, DrawColorSet colorSet = DrawColorSet.Default, int width = 1)
		{
			Draw(content, width, colorSet);
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

		private static void ChangeToSelectedColor()
		{
			Console.ForegroundColor = Settings.SelectedForegroundColor;
			Console.BackgroundColor = Settings.DefaultBackgroundColor;
		}

		private static Table CreateViewTable(Table table, ViewOptions viewOptions)
		{
			var viewTable = JsonConvert.DeserializeObject<Table>(JsonConvert.SerializeObject(table));
			viewTable.Cells = viewTable[viewOptions.Position.X, viewOptions.Position.Y, viewOptions.Position.X + viewOptions.Size.Width - 1, viewOptions.Position.Y + viewOptions.Size.Height - 1];
			viewTable.Size = new Size(viewOptions.Size.Width, viewOptions.Size.Height);

			viewTable.AddRowAt(0);

			for (int x = 0; x < viewTable.Size.Width; x++)
			{
				viewTable[x, 0].Content = viewOptions.Position.X + x;
				viewTable[x, 0].IsSelected = viewTable[x, SelectType.Column].Any(c => c.IsSelected);
			}

			viewTable.AddColumnAt(0);

			viewTable[0, 0].Content = @"y \ x";

			for (int y = 1; y < viewTable.Size.Height; y++)
			{
				viewTable[0, y].Content = viewOptions.Position.Y + y - 1;
				viewTable[0, y].IsSelected = viewTable[y, SelectType.Row].Any(c => c.IsSelected);

			}

			return viewTable;
		}

		private enum DrawColorSet
		{
			Default,
			Border,
			SelectedBorder
		}
	}
}
