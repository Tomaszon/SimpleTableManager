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
		private static List<int> _ROW_HEIGHTS;

		public static void Render(Table table, ViewOptions viewOptions = null)
		{
			viewOptions ??= new ViewOptions(0, 0, table.Size.Width, table.Size.Height);

			var viewTable = CreateViewTable(table, viewOptions);

			_COLUMN_WIDTHS = new List<int>();
			for (int i = 0; i < viewTable.Size.Width; i++)
			{
				_COLUMN_WIDTHS.Add(viewTable.GetColumnWidth(i));
			}

			_ROW_HEIGHTS = new List<int> { 1 };
			for (int i = 0; i < viewTable.Size.Height - 1; i++)
			{
				_ROW_HEIGHTS.Add(i % 2 == 0 ? 3 : 4);
			}

			DrawSeparatorLine(0, viewTable.Size.Width, viewTable.Size.Height);

			for (int y = 0; y < viewTable.Size.Height; y++)
			{
				DrawContentRow(viewTable, y);
			}

			ChangeToDefaultColors();
		}

		private static void DrawContentRow(Table viewTable, int index)
		{
			for (int i = 0; i < _ROW_HEIGHTS[index]; i++)
			{
				DrawContentLine(i, viewTable[0, index, viewTable.Size.Width - 1, index], index, viewTable.Size.Height, viewTable.Size.Width);
			}

			DrawSeparatorLine(index + 1, viewTable.Size.Width, viewTable.Size.Height);
		}

		private static void DrawSeparatorLine(int borderRowNo, int columnCount, int rowCount)
		{
			Draw(GetTableCharacter(borderRowNo, 0, rowCount, columnCount), DrawColorSet.Border);

			for (int i = 0; i < columnCount; i++)
			{
				Draw(new string(GetTableCharacter(borderRowNo, i + 0.5m, rowCount, columnCount), _COLUMN_WIDTHS[i]), DrawColorSet.Border);
				Draw(GetTableCharacter(borderRowNo, i + 1, rowCount, columnCount), DrawColorSet.Border);
			}

			Console.WriteLine();
		}

		private static void DrawContentLine(int index, List<Cell> cells, int borderRowNo, int rowCount, int columnCount)
		{
			Draw(GetTableCharacter(borderRowNo + 0.5m, 0, rowCount, columnCount), DrawColorSet.Border);

			for (int i = 0; i < cells.Count; i++)
			{
				//TODO draws only one row for the cell, broken
				if (cells[i].VertialAlignment == VertialAlignment.Top && index == 0 ||
					cells[i].VertialAlignment == VertialAlignment.Bottom && index == _ROW_HEIGHTS[borderRowNo] - 1 ||
					cells[i].VertialAlignment == VertialAlignment.Center && index == (_ROW_HEIGHTS[borderRowNo] - 1) / 2)
				{
					Draw(cells[i], index, _COLUMN_WIDTHS[i]);
				}
				else
				{
					Draw("", _COLUMN_WIDTHS[i]);
				}

				Draw(GetTableCharacter(borderRowNo + 0.5m, i + 1, rowCount, columnCount), DrawColorSet.Border);
			}

			Console.WriteLine();
		}

		private static void Draw(Cell cell, int index, int width)
		{
			if (cell.Content.Count > index)
			{
				if (cell.IsSelected)
				{
					ChangeToSelectedColor();
				}
				else
				{
					ChangeToDefaultColors();
				}

				var content = cell.Content[index].ToString();

				switch (cell.HorizontalAlignment)
				{
					case HorizontalAlignment.Left:
						Console.Write(content.PadRight(width));
						break;
					case HorizontalAlignment.Center:
						Console.Write(content.PadLeftRight(width));
						break;
					case HorizontalAlignment.Right:
						Console.Write(content.PadLeft(width));
						break;
				}

				ChangeToDefaultColors();
			}
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
			var content = JsonConvert.SerializeObject(table, Formatting.Indented);
			var viewTable = JsonConvert.DeserializeObject<Table>(content);
			viewTable.Cells = viewTable[viewOptions.Position.X, viewOptions.Position.Y, viewOptions.Position.X + viewOptions.Size.Width - 1, viewOptions.Position.Y + viewOptions.Size.Height - 1];
			viewTable.Size = new Size(viewOptions.Size.Width, viewOptions.Size.Height);

			viewTable.AddRowAt(0);

			for (int x = 0; x < viewTable.Size.Width; x++)
			{
				viewTable[x, 0].ContentType = typeof(int);
				viewTable[x, 0].SetContent(viewOptions.Position.X + x);
				viewTable[x, 0].IsSelected = viewTable[x, SelectType.Column].Any(c => c.IsSelected);
			}

			viewTable.AddColumnAt(0);

			viewTable[0, 0].SetContent(@"y \ x");

			for (int y = 1; y < viewTable.Size.Height; y++)
			{
				viewTable[0, y].ContentType = typeof(int);
				viewTable[0, y].SetContent(viewOptions.Position.Y + y - 1);
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
				var up = GetNextTableCharacter(borderRowNo > 0, borderColNo < 2, TableBorderCharacterMode.Up);
				var left = GetNextTableCharacter(borderColNo > 0, borderRowNo < 2, TableBorderCharacterMode.Left);
				var right = GetNextTableCharacter(borderColNo < columnCount, borderRowNo < 2, TableBorderCharacterMode.Right);
				var down = GetNextTableCharacter(borderRowNo < rowCount, borderColNo < 2, TableBorderCharacterMode.Down);

				return TableBorderCharacters.Get(up | left | right | down);
			}
			else if (betweenColumns)
			{
				return TableBorderCharacters.Get(GetNextTableCharacter(true, borderRowNo < 2, TableBorderCharacterMode.Horizontal));
			}
			else if (betweenRows)
			{
				return TableBorderCharacters.Get(GetNextTableCharacter(true, borderColNo < 2, TableBorderCharacterMode.Vertical));
			}

			throw new InvalidOperationException();
		}

		private static TableBorderCharacterMode GetNextTableCharacter(bool caseSingle, bool caseDouble, TableBorderCharacterMode value)
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
