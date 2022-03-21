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
			DrawSeparatorLine(1, viewTable.Size.Width, viewTable.Size.Height);

			DrawContentLine(viewTable[0, 0, viewTable.Size.Width - 1, 0]);

			DrawSeparatorLine(2, viewTable.Size.Width, viewTable.Size.Height);
		}

		private static void DrawContentRow(Table viewTable, int index)
		{
			DrawContentLine(viewTable[0, index, viewTable.Size.Width - 1, index]);

			DrawSeparatorLine(index == viewTable.Size.Height - 1 ? 4 : 3, viewTable.Size.Width, viewTable.Size.Height);
		}

		private static void DrawSeparatorLine(int lineNo, int columnCount, int rowCount)
		{
			Draw(GetTableCharacter(lineNo, 1, rowCount, columnCount), DrawColorSet.Border);

			for (int i = 0; i < columnCount; i++)
			{
				Draw(new string(GetTableCharacter(lineNo, Enumerable.Average(new[] { i + 1, (decimal)i }), rowCount, columnCount), _COLUMN_WIDTHS[i]), DrawColorSet.Border);
				Draw(GetTableCharacter(lineNo, i + 1, rowCount, columnCount), DrawColorSet.Border);
			}



			//Draw(TableBorderCharacters.Get(node1), DrawColorSet.Border);
			//Draw(new string(TableBorderCharacters.Get(normal), _COLUMN_WIDTHS[0]), DrawColorSet.Border);
			//Draw(TableBorderCharacters.Get(node2), DrawColorSet.Border);

			//if (columnCount > 1)
			//{
			//	for (int i = 1; i < columnCount - 1; i++)
			//	{
			//		Draw(new string(TableBorderCharacters.Get(normal), _COLUMN_WIDTHS[i]), DrawColorSet.Border);
			//		Draw(TableBorderCharacters.Get(node3), DrawColorSet.Border);
			//	}

			//	Draw(new string(TableBorderCharacters.Get(normal), _COLUMN_WIDTHS.Last()), DrawColorSet.Border);
			//	Draw(TableBorderCharacters.Get(node4), DrawColorSet.Border);
			//}

			Console.WriteLine();
		}

		private static void DrawContentLine(List<Cell> cells)
		{
			//Draw(TableBorderCharacters.Get(TableBorderCharacter.DU_DD), DrawColorSet.Border);
			//Draw(cells[0], _COLUMN_WIDTHS[0]);
			//Draw(TableBorderCharacters.Get(TableBorderCharacter.DU_DD), DrawColorSet.Border);

			//for (int i = 1; i < cells.Count; i++)
			//{
			//	Draw(cells[i], _COLUMN_WIDTHS[i]);
			//	Draw(TableBorderCharacters.Get(TableBorderCharacter.SU_SD), DrawColorSet.Border);
			//}
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

		private static char GetTableCharacter(decimal borderRowNo, decimal borderColNo, int rowCount, int columnCount)
		{
			var betweenColumns = (int)borderColNo != borderColNo;
			var betweenRows = (int)borderRowNo != borderRowNo;

			if (!betweenColumns && !betweenRows)
			{
				//TODO
				return 'O';
			}
			else if (betweenColumns)
			{
				var isDouble = borderRowNo <= 2;

				return isDouble ? TableBorderCharacters.Get(TableBorderCharacterMode.LeftDouble | TableBorderCharacterMode.RightDouble) :
					TableBorderCharacters.Get(TableBorderCharacterMode.Left | TableBorderCharacterMode.Right);
			}
			else if (betweenRows)
			{
				var isDouble = borderColNo <= 2;

				return isDouble ? TableBorderCharacters.Get(TableBorderCharacterMode.UpDouble | TableBorderCharacterMode.DownDouble) :
					TableBorderCharacters.Get(TableBorderCharacterMode.Up | TableBorderCharacterMode.Down);
			}

			throw new InvalidOperationException();
		}

		private enum DrawColorSet
		{
			Default,
			Border,
			SelectedBorder
		}
	}
}
