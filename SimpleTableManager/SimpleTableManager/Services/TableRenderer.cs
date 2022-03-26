using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class TableRenderer
	{
		private static List<int> _COLUMN_WIDTHS;
		private static List<int> _ROW_HEIGHTS;
		private static int _TABLE_HORIZONTAL_OFFSET = 0;
		private static Size _TABLE_SIZE;

		public static void Render(Table table, ViewOptions viewOptions = null)
		{
			viewOptions ??= new ViewOptions(0, 0, table.Size.Width, table.Size.Height);

			var viewTable = CreateViewTable(table, viewOptions);

			_COLUMN_WIDTHS = viewTable.GetColumnWidths();

			_ROW_HEIGHTS = viewTable.GetRowHeights();

			_TABLE_SIZE = new Size(_COLUMN_WIDTHS.Sum() + _COLUMN_WIDTHS.Count + 1, _ROW_HEIGHTS.Sum() + _ROW_HEIGHTS.Count + 1);

			_TABLE_HORIZONTAL_OFFSET = GetTableDisplayOffset();

			DrawSeparatorLine(0, viewTable.Size.Width, viewTable.Size.Height);

			for (int y = 0; y < viewTable.Size.Height; y++)
			{
				DrawContentRow(viewTable, y);
			}

			ChangeToDefaultColors();
		}

		private static int GetTableDisplayOffset()
		{
			return (Console.WindowWidth - _TABLE_SIZE.Width) / 2;
		}

		private static void DrawContentRow(Table viewTable, int rowIndex)
		{
			for (int i = 0; i < _ROW_HEIGHTS[rowIndex]; i++)
			{
				DrawContentLine(i, viewTable[0, rowIndex, viewTable.Size.Width - 1, rowIndex], rowIndex, viewTable.Size.Height, viewTable.Size.Width);
			}

			DrawSeparatorLine(rowIndex + 1, viewTable.Size.Width, viewTable.Size.Height);

			Task.Delay(Settings.RenderDelay).Wait();
		}

		private static void DrawSeparatorLine(int borderRowNo, int columnCount, int rowCount)
		{
			Console.SetCursorPosition(_TABLE_HORIZONTAL_OFFSET, Console.CursorTop);

			Draw(GetTableCharacter(borderRowNo, 0, rowCount, columnCount), DrawColorSet.Border);

			for (int i = 0; i < columnCount; i++)
			{
				Draw(new string(GetTableCharacter(borderRowNo, i + 0.5m, rowCount, columnCount), _COLUMN_WIDTHS[i]), DrawColorSet.Border);
				Draw(GetTableCharacter(borderRowNo, i + 1, rowCount, columnCount), DrawColorSet.Border);
			}

			Console.WriteLine();
		}

		private static void DrawContentLine(int lineIndex, List<Cell> cells, int rowIndex, int rowCount, int columnCount)
		{
			Console.SetCursorPosition(_TABLE_HORIZONTAL_OFFSET, Console.CursorTop);

			Draw(GetTableCharacter(rowIndex + 0.5m, 0, rowCount, columnCount), DrawColorSet.Border);

			for (int i = 0; i < cells.Count; i++)
			{
				if (IsCellContentDrawNeeded(cells[i], lineIndex, rowIndex, out var contentIndex))
				{
					Draw(cells[i], contentIndex, _COLUMN_WIDTHS[i]);
				}
				else
				{
					Draw("", _COLUMN_WIDTHS[i]);
				}

				Draw(GetTableCharacter(rowIndex + 0.5m, i + 1, rowCount, columnCount), DrawColorSet.Border);
			}

			Console.WriteLine();
		}

		private static bool IsCellContentDrawNeeded(Cell cell, int lineIndex, int rowIndex, out int contentIndex)
		{
			var ch = cell.Content.Count;

			var rh = _ROW_HEIGHTS[rowIndex];

			var startLineIndex = cell.VertialAlignment switch
			{
				VertialAlignment.Top => 0,
				VertialAlignment.Bottom => rh - ch,
				_ => (rh - ch) / 2
			};

			var isDrawNeeded = startLineIndex <= lineIndex && startLineIndex + cell.Content.Count > lineIndex;

			contentIndex = isDrawNeeded ? Math.Abs(startLineIndex - lineIndex) : default;

			return isDrawNeeded;
		}

		private static void Draw(Cell cell, int contentIndex, int width)
		{
			if (cell.Content.Count > contentIndex)
			{
				var content = cell.Content[contentIndex].ToString();

				switch (cell.HorizontalAlignment)
				{
					case HorizontalAlignment.Left:
						content = content.PadRight(width);
						break;
					case HorizontalAlignment.Center:
						content = content.PadLeftRight(width);
						break;
					case HorizontalAlignment.Right:
						content = content.PadLeft(width);
						break;
				}

				Draw(content, 0, cell.IsSelected ? DrawColorSet.SelectedBorder : DrawColorSet.Default);
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
				viewTable[x, 0].IsSelected = viewTable.GetColumn(x).Any(c => c.IsSelected);
			}

			viewTable.AddColumnAt(0);

			viewTable[0, 0].SetContent(@"y \ x");

			for (int y = 1; y < viewTable.Size.Height; y++)
			{
				viewTable[0, y].ContentType = typeof(int);
				viewTable[0, y].SetContent(viewOptions.Position.Y + y - 1);
				viewTable[0, y].IsSelected = viewTable.GetRow(y).Any(c => c.IsSelected);
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
