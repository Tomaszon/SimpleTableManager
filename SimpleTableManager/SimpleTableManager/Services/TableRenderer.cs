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
		private static Position _TABLE_OFFSET;
		private static Size _VIEW_TABLE_GRAPHICAL_SIZE;
		private static Table _VIEW_TABLE;

		public static void Render(Table table)
		{
			ChangeToDefaultCellColors();

			Console.WriteLine($"{table.Name}\n");

			do
			{
				CreateViewTable(table);

				_COLUMN_WIDTHS = _VIEW_TABLE.GetColumnWidths();

				_ROW_HEIGHTS = _VIEW_TABLE.GetRowHeights();

				_VIEW_TABLE_GRAPHICAL_SIZE = new Size(_COLUMN_WIDTHS.Sum() + _COLUMN_WIDTHS.Count + 1, _ROW_HEIGHTS.Sum() + _ROW_HEIGHTS.Count + 1);

				_TABLE_OFFSET = new Position((Console.WindowWidth - _VIEW_TABLE_GRAPHICAL_SIZE.Width) / 2, Console.CursorTop);
			}
			while (IsViewTableShrinkNeeded(table));

			Console.SetCursorPosition(0, _TABLE_OFFSET.Y);

			//TODO resolve this mess
			//--------------
			Random rand = new Random();


			int fadeSize = 5;
			var lorem = "";

			for (int y = 0; y < _VIEW_TABLE_GRAPHICAL_SIZE.Height; y++)
			{
				lorem += new string(' ', _TABLE_OFFSET.X);
				for (int x = 0; x < _VIEW_TABLE_GRAPHICAL_SIZE.Width; x++)
				{
					if (x < fadeSize || y < fadeSize)
					{
						if (rand.Next(0, Math.Min(x + 1, y + 1) + 1) == 0)
						{
							lorem += ' ';
						}
						else
						{
							lorem += (char)rand.Next(' ' + 1, 'Z');
						}
					}
					else if (x > _VIEW_TABLE_GRAPHICAL_SIZE.Width - fadeSize - 1 || y > _VIEW_TABLE_GRAPHICAL_SIZE.Height - fadeSize - 1)
					{
						if (rand.Next(0, Math.Min(_VIEW_TABLE_GRAPHICAL_SIZE.Width - x + 1, _VIEW_TABLE_GRAPHICAL_SIZE.Height - y + 1) + 1) == 0)
						{
							lorem += ' ';
						}
						else
						{
							lorem += (char)rand.Next(' ' + 1, 'Z');
						}
					}
					else
					{
						lorem += (char)rand.Next(' ' + 1, 'Z');
					}
				}
				lorem += ('\n');
			}

			Console.Write(lorem);



			//--------------

			Console.SetCursorPosition(_TABLE_OFFSET.X, _TABLE_OFFSET.Y);

			DrawSeparatorLine(0, _VIEW_TABLE.Size.Width, _VIEW_TABLE.Size.Height);

			for (int y = 0; y < _VIEW_TABLE.Size.Height; y++)
			{
				DrawContentRow(y);
			}

			ChangeToTextColors();
		}

		private static bool IsViewTableShrinkNeeded(Table table)
		{
			int freeLinesForInput = 16;

			if (Console.WindowWidth - _VIEW_TABLE_GRAPHICAL_SIZE.Width < 0)
			{
				table.ViewOptions.DecreaseWidth();

				return true;
			}
			else if (Console.WindowHeight - freeLinesForInput - _VIEW_TABLE_GRAPHICAL_SIZE.Height < 0)
			{
				table.ViewOptions.DecreaseHeight();

				return true;
			}

			return false;
		}

		private static void DrawContentRow(int rowIndex)
		{
			for (int i = 0; i < _ROW_HEIGHTS[rowIndex]; i++)
			{
				DrawContentLine(i, _VIEW_TABLE[0, rowIndex, _VIEW_TABLE.Size.Width - 1, rowIndex], rowIndex, _VIEW_TABLE.Size.Height, _VIEW_TABLE.Size.Width);
			}

			DrawSeparatorLine(rowIndex + 1, _VIEW_TABLE.Size.Width, _VIEW_TABLE.Size.Height);

			//if (i % 10 == 0)
			{
				Task.Delay(50 / _VIEW_TABLE.Size.Width).Wait();
			}
		}

		private static void DrawSeparatorLine(int borderRowNo, int columnCount, int rowCount)
		{
			Console.SetCursorPosition(_TABLE_OFFSET.X, Console.CursorTop);

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
			Console.SetCursorPosition(_TABLE_OFFSET.X, Console.CursorTop);

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
						content = content.PadRight(width - 1).PadLeft(width);
						break;
					case HorizontalAlignment.Center:
						content = content.PadLeftRight(width);
						break;
					case HorizontalAlignment.Right:
						content = content.PadLeft(width - 1).PadRight(width);
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
					ChangeToDefaultCellColors();
					break;

				case DrawColorSet.Border:
					ChangeToBorderColors();
					break;

				case DrawColorSet.SelectedBorder:
					ChangeToSelectedCellColors();
					break;
			}

			var formattedContent = content.ToString().PadLeftRight(width);

			//foreach (var c in formattedContent)
			//{
			//	Console.Write(c);
			//	Task.Delay(10).Wait();
			//}

			Console.Write(formattedContent);
		}

		private static void Draw(object content, DrawColorSet colorSet = DrawColorSet.Default, int width = 1)
		{
			Draw(content, width, colorSet);
		}

		private static void ChangeToDefaultCellColors()
		{
			Console.ForegroundColor = Settings.Current.DefaultCellForegroundColor;
			Console.BackgroundColor = Settings.Current.DefaultCellBackgroundColor;
		}

		private static void ChangeToBorderColors()
		{
			Console.ForegroundColor = Settings.Current.BorderForegroundColor;
			Console.BackgroundColor = Settings.Current.BorderBackgroundColor;
		}

		private static void ChangeToSelectedCellColors()
		{
			Console.ForegroundColor = Settings.Current.SelectedCellForegroundColor;
			Console.BackgroundColor = Settings.Current.SelectedCellBackgroundColor;
		}

		private static void ChangeToTextColors()
		{
			Console.ForegroundColor = Settings.Current.TextForegroundColor;
			Console.BackgroundColor = Settings.Current.TextBackgroundColor;
		}

		private static void CreateViewTable(Table table)
		{
			var content = JsonConvert.SerializeObject(table, Formatting.Indented);
			_VIEW_TABLE = JsonConvert.DeserializeObject<Table>(content);
			var voStartPosition = _VIEW_TABLE.ViewOptions.StartPosition;
			var voEndPosition = _VIEW_TABLE.ViewOptions.EndPosition;

			var leftEllipsis = voStartPosition.X > 0;
			var upEllipsis = voStartPosition.Y > 0;
			var rightEllipsis = voEndPosition.X < table.Size.Width - 1;
			var downEllipsis = voEndPosition.Y < table.Size.Height - 1;

			_VIEW_TABLE.Cells = _VIEW_TABLE[voStartPosition.X, voStartPosition.Y, voEndPosition.X, voEndPosition.Y];
			_VIEW_TABLE.Size = _VIEW_TABLE.ViewOptions.Size;

			_VIEW_TABLE.AddRowAt(0);

			for (int x = 0; x < _VIEW_TABLE.Size.Width; x++)
			{
				var colIndex = _VIEW_TABLE.ViewOptions.StartPosition.X + x;

				_VIEW_TABLE[x, 0].ContentType = typeof(string);

				var left = leftEllipsis && x == 0 ? "0 ◀ " : "";
				var right = rightEllipsis && x == _VIEW_TABLE.Size.Width - 1 ? $" ▶ {table.Size.Width - 1}" : "";

				_VIEW_TABLE[x, 0].SetContent($"{left}{colIndex}{right}");

				_VIEW_TABLE[x, 0].IsSelected = _VIEW_TABLE.GetColumn(x).Any(c => c.IsSelected);
			}

			_VIEW_TABLE.AddColumnAt(0);

			_VIEW_TABLE[0, 0].SetContent(@"y \ x");

			for (int y = 1; y < _VIEW_TABLE.Size.Height; y++)
			{
				var rowIndex = _VIEW_TABLE.ViewOptions.StartPosition.Y + y - 1;

				_VIEW_TABLE[0, y].ContentType = typeof(string);

				var left = upEllipsis && y == 1 ? "0 ▲ " : "";
				var rigth = downEllipsis && y == _VIEW_TABLE.Size.Height - 1 ? $" ▼ {table.Size.Height - 1}" : "";

				_VIEW_TABLE[0, y].SetContent($"{left}{rowIndex}{rigth}");

				_VIEW_TABLE[0, y].IsSelected = _VIEW_TABLE.GetRow(y).Any(c => c.IsSelected);
			}
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
