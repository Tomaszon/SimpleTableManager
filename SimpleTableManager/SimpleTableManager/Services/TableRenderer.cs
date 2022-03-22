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
			DrawSeparatorLine(0, viewTable.Size.Width, viewTable.Size.Height);

			DrawContentLine(viewTable[0, 0, viewTable.Size.Width - 1, 0]);

			DrawSeparatorLine(1, viewTable.Size.Width, viewTable.Size.Height);
		}

		private static void DrawContentRow(Table viewTable, int index)
		{
			DrawContentLine(viewTable[0, index, viewTable.Size.Width - 1, index]);

			DrawSeparatorLine(index + 1, viewTable.Size.Width, viewTable.Size.Height);
		}

		private static void DrawSeparatorLine(int borderRowNo, int columnCount, int rowCount)
		{
			Draw(GetTableCharacter(borderRowNo, 0, rowCount, columnCount), DrawColorSet.Border);

			for (int i = 0; i < columnCount; i++)
			{
				Draw(new string(GetTableCharacter(borderRowNo, Enumerable.Average(new[] { i + 1, (decimal)i }), rowCount, columnCount), _COLUMN_WIDTHS[i]), DrawColorSet.Border);
				Draw(GetTableCharacter(borderRowNo, i + 1, rowCount, columnCount), DrawColorSet.Border);
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
				var up = GetChar(borderRowNo > 0, borderColNo < 2, TableBorderCharacterMode.Up);
				var left = GetChar(borderColNo > 0, borderRowNo < 2, TableBorderCharacterMode.Left);
				var right = GetChar(borderColNo < columnCount, borderRowNo < 2, TableBorderCharacterMode.Right);
				var down = GetChar(borderRowNo < rowCount, borderColNo < 2, TableBorderCharacterMode.Down);

				return TableBorderCharacters.Get(up | left | right | down);
			}
			else if (betweenColumns)
			{
				return TableBorderCharacters.Get(GetChar(true, borderRowNo < 2, TableBorderCharacterMode.Horizontal));
			}
			else if (betweenRows)
			{
				return TableBorderCharacters.Get(GetChar(true, borderColNo < 2, TableBorderCharacterMode.Vertical));
			}

			throw new InvalidOperationException();
		}

		private static TableBorderCharacterMode GetChar(bool caseSingle, bool caseDouble, TableBorderCharacterMode value)
		{
			if (caseSingle && caseDouble)
			{
				return (TableBorderCharacterMode)((int)value * 2);
			}
			else if (caseSingle)
			{
				return value;
			}

			return TableBorderCharacterMode.None;
		}

		private enum DrawColorSet
		{
			Default,
			Border,
			SelectedBorder
		}
	}
}
