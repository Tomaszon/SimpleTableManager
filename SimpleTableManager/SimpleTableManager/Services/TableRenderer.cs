using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class TableRenderer
	{
		//private static List<int> _COLUMN_WIDTHS;
		//private static List<int> _ROW_HEIGHTS;
		//private static Position _TABLE_OFFSET;
		//private static Size _VIEW_TABLE_GRAPHICAL_SIZE;
		//private static Table _VIEW_TABLE;

		public static void Render(Table table)
		{
			ChangeToTextColors();

			Console.WriteLine($"{table.Name}\n");

			var tableSize = table.GetTableSize();

			var tableOffset = new Size((Console.WindowWidth - tableSize.Width) / 2, 10);

			RenderTempCell(tableOffset, tableSize);

			RenderCornerCell(table, tableOffset);

			RenderHeader(table, tableOffset);

			RenderSider(table, tableOffset);

			RenderContent(table, tableOffset);

			Console.WriteLine();

			ChangeToTextColors();
		}

		private static void RenderTempCell(Size tableOffset, Size size)
		{
			var placeHolderCell = new Cell(GetTmpBackground(size)) { ForegroundColor = Settings.Current.TextForegroundColor };
			var placeHolderCellPosition = new Position(tableOffset);

			DrawCellBorder(placeHolderCell, placeHolderCellPosition, size, CellBorders.Get(CellBorderName.CornerCellClosed));

			DrawCellContent(placeHolderCell, placeHolderCellPosition, size);

			Task.Delay(250).Wait();
		}

		private static string[] GetTmpBackground(Size size)
		{
			var pattern1 = ".   ";
			var pattern2 = "  . ";

			string line1 = GetLine(size, pattern1);
			string line2 = GetLine(size, pattern2);

			var result = new List<string>();

			Shared.IndexArray((int)Math.Ceiling((size.Height - 2) / 2m)).ForEach(i =>
			{
				result.Add(line1);
				result.Add(line2);
			});

			return result.Take(size.Height - 2).ToArray();

			static string GetLine(Size size, string value)
			{
				return Shared.IndexArray((int)Math.Ceiling((size.Width - 2) / (decimal)value.Length))
					.Aggregate("", (s, i) => s += value)[..(size.Width - 2)];
			}
		}

		private static void RenderCornerCell(Table table, Size tableOffset)
		{
			var position = new Position(tableOffset.Width, tableOffset.Height);

			var size = table.GetCornerCellSize();

			DrawCellBorder(table.CornerCell, position, size, CellBorders.Get(CellBorderName.CornerCellOpen));

			DrawCellContent(table.CornerCell, position, size);
		}

		private static void RenderHeader(Table table, Size tableOffset)
		{
			Shared.IndexArray(table.Size.Width).ForEach(x =>
			{
				var cell = table.Header[x];

				if (!cell.IsHidden && table.IsCellInView(x: x))
				{
					var position = table.GetHeaderCellPosition(tableOffset, x);

					var size = table.GetHeaderCellSize(x);

					DrawCellBorder(cell, position, size, x < table.Size.Width - 1 ?
						CellBorders.Get(CellBorderName.HeaderOpen) : CellBorders.Get(CellBorderName.HeaderVertical).Trim(left: true));

					DrawCellContent(cell, position, size);
				}
			}, 10);
		}

		private static void RenderSider(Table table, Size tableOffset)
		{
			Shared.IndexArray(table.Size.Height).ForEach(y =>
			{
				var cell = table.Sider[y];
				if (!cell.IsHidden && table.IsCellInView(y: y))
				{
					var position = table.GetSiderCellPosition(tableOffset, y);

					var size = table.GetSiderCellSize(y);

					DrawCellBorder(cell, position, size, y < table.Size.Height - 1 ?
						CellBorders.Get(CellBorderName.SiderOpen) : CellBorders.Get(CellBorderName.SiderHorizontal).Trim(top: true));

					DrawCellContent(cell, position, size);
				}
			}, 10);
		}

		private static void RenderContent(Table table, Size tableOffset)
		{
			Shared.IndexArray(table.Size.Height).ForEach(y =>
				Shared.IndexArray(table.Size.Width).ForEach(x =>
				{
					var cell = table[x, y];

					if (!cell.IsHidden && table.IsCellInView(x, y))
					{
						var position = table.GetContentCellPosition(tableOffset, x, y);

						var size = table.GetContentCellSize(x, y);

						var cellBorderName = CellBorderName.ContentOpen;

						if (x == table.Size.Width - 1 && y == table.Size.Height - 1)
						{
							cellBorderName = CellBorderName.ContentUpLeft;
						}
						else if (x == table.Size.Width - 1)
						{
							cellBorderName = CellBorderName.ContentVerticalLeft;
						}
						else if (y == table.Size.Height - 1)
						{
							cellBorderName = CellBorderName.ContentHorizontalUp;
						}

						var border = CellBorders.Get(cellBorderName);

						if (x == 0 || x > 0 && table[x - 1, y].IsSelected && !cell.IsSelected)
						{
							border = border.Trim(left: true);
						}
						if (y == 0 || y > 0 && table[x, y - 1].IsSelected && !cell.IsSelected)
						{
							border = border.Trim(top: true);
						}
						if (x < table.Size.Width - 1 && table[x + 1, y].IsSelected && !cell.IsSelected)
						{
							border = border.Trim(right: true);
						}
						if (y < table.Size.Height - 1 && table[x, y + 1].IsSelected && !cell.IsSelected)
						{
							border = border.Trim(bottom: true);
						}

						DrawCellBorder(cell, position, size, border);

						DrawCellContent(cell, position, size);
					}
				}));
		}

		private static void DrawCellBorder(Cell cell, Position position, Size size, CellBorder border)
		{
			Console.SetCursorPosition(position.X + 1, position.Y);

			ChangeToCellBorderColors(cell);

			DrawBorderSegment(border.Top, size.Width - 2);

			Shared.StepCursor(-(size.Width - 1), 1);
			Shared.IndexArray(size.Height - 2, 1).ForEach(i =>
			{
				DrawBorderSegment(border.Left);

				Shared.StepCursor(size.Width - 2, 0);

				DrawBorderSegment(border.Right);
				Shared.StepCursor(-size.Width, 1);
			});

			Shared.StepCursor(1, 0);

			DrawBorderSegment(border.Bottom, size.Width - 2);

			Console.SetCursorPosition(position.X, position.Y);

			ChangeToTextColors();

			DrawBorderSegment(border.TopLeft);

			Shared.StepCursor(size.Width - 2, 0);

			DrawBorderSegment(border.TopRight);

			Shared.StepCursor(-size.Width, size.Height - 1);

			DrawBorderSegment(border.BottomLeft);

			Shared.StepCursor(size.Width - 2, 0);

			DrawBorderSegment(border.BottomRight);
		}

		private static void DrawBorderSegment(BorderType border, int count = 1)
		{
			if (border != BorderType.None)
			{
				Console.Write(new string(BorderCharacters.Get(border), count));
			}
			else
			{
				Shared.StepCursor(count, 0);
			}
		}

		private static void DrawCellContent(Cell cell, Position position, Size size)
		{
			var sizeWithoutBorders = new Size(size.Width - 2, size.Height - 2);

			Shared.IndexArray(sizeWithoutBorders.Height).ForEach(i =>
			{
				Console.SetCursorPosition(position.X + 1, position.Y + 1 + i);

				var content = new string(' ', sizeWithoutBorders.Width);

				if (IsCellContentDrawNeeded(cell, i, sizeWithoutBorders.Height, out var contentIndex) && cell.Content.Count > contentIndex)
				{
					content = cell.Content[contentIndex].ToString();

					if (!string.IsNullOrWhiteSpace(content))
					{
						switch (cell.HorizontalAlignment)
						{
							case HorizontalAlignment.Left:
								{
									content = content.AppendLeft(' ', cell.Padding.Left);
									content = content.PadRight(sizeWithoutBorders.Width);
								}
								break;

							case HorizontalAlignment.Center:
								{
									var startIndex = GetStartIndexForCenteredContent(sizeWithoutBorders.Width, content.Length, cell.Padding.Left, cell.Padding.Right);

									content = content.AppendLeft(' ', startIndex);
									content = content.PadRight(sizeWithoutBorders.Width);
								}
								break;

							case HorizontalAlignment.Right:
								{
									content = content.AppendRight(' ', cell.Padding.Right);
									content = content.PadLeft(sizeWithoutBorders.Width);
								}
								break;
						}
					}
				}

				ChangeToCellContentColors(cell);

				Console.Write(content);
			});
		}

		//private static bool IsViewTableShrinkNeeded(Table table)
		//{
		//int freeLinesForInput = 16;

		//if (Console.WindowWidth - _VIEW_TABLE_GRAPHICAL_SIZE.Width < 0)
		//{
		//	table.ViewOptions.DecreaseWidth();

		//	return true;
		//}
		//else if (Console.WindowHeight - freeLinesForInput - _VIEW_TABLE_GRAPHICAL_SIZE.Height < 0)
		//{
		//	table.ViewOptions.DecreaseHeight();

		//	return true;
		//}

		//	return false;
		//}

		//private static void DrawSeparatorLine(int rowIndex, int columnCount, int rowCount)
		//{
		//	//TODO fix coloring
		//	Console.SetCursorPosition(_TABLE_OFFSET.X, Console.CursorTop);

		//	ChangeToBorderColors(false);

		//	Draw(GetTableCharacter(rowIndex, 0, rowCount, columnCount));

		//	for (int x = 0; x < columnCount; x++)
		//	{
		//		ChangeToBorderColors(IsHorizontalBorderSegmentSelected(x, rowIndex));

		//		Draw(new string(GetTableCharacter(rowIndex, x + 0.5m, rowCount, columnCount), _COLUMN_WIDTHS[x]));

		//		//var isSelected = _VIEW_TABLE.IsColumnSelected(x + 1) &&
		//		//	(_VIEW_TABLE.IsRowSelected(rowIndex) || _VIEW_TABLE.IsRowSelected(rowIndex - 1));

		//		ChangeToBorderColors(IsCornerBorderSegmentSelected(x, rowIndex));

		//		Draw(GetTableCharacter(rowIndex, x + 1, rowCount, columnCount));
		//	}

		//	Console.WriteLine();
		//}

		//private static void DrawContentLine(int lineIndex, List<Cell> cells, int rowIndex, int rowCount)
		//{
		//	Console.SetCursorPosition(_TABLE_OFFSET.X, Console.CursorTop);

		//	ChangeToBorderColors(false);

		//	Draw(GetTableCharacter(rowIndex + 0.5m, 0, rowCount, cells.Count));

		//	for (int i = 0; i < cells.Count; i++)
		//	{
		//		Draw(cells[i], lineIndex, rowIndex, _COLUMN_WIDTHS[i]);

		//		ChangeToBorderColors(cells[i].IsSelected || i + 1 < cells.Count && cells[i + 1].IsSelected);

		//		Draw(GetTableCharacter(rowIndex + 0.5m, i + 1, rowCount, cells.Count));
		//	}

		//	Console.WriteLine();
		//}

		//private static bool IsHorizontalBorderSegmentSelected(int columnIndex, int rowIndex)
		//{
		//	return _VIEW_TABLE.IsCellSelected(columnIndex, rowIndex) || _VIEW_TABLE.IsCellSelected(columnIndex, rowIndex - 1);
		//}

		//private static bool IsCornerBorderSegmentSelected(int columnIndex, int rowIndex)
		//{
		//	return _VIEW_TABLE.IsCellSelected(columnIndex, rowIndex) || _VIEW_TABLE.IsCellSelected(columnIndex, rowIndex - 1) ||
		//		_VIEW_TABLE.IsCellSelected(columnIndex + 1, rowIndex) || _VIEW_TABLE.IsCellSelected(columnIndex + 1, rowIndex - 1);
		//}

		private static bool IsCellContentDrawNeeded(Cell cell, int lineIndex, int height, out int contentIndex)
		{
			var startLineIndex = cell.VertialAlignment switch
			{
				VertialAlignment.Top => cell.Padding.Top,
				VertialAlignment.Center => GetStartIndexForCenteredContent(height, cell.Content.Count, cell.Padding.Top, cell.Padding.Bottom),
				_ => height - cell.Content.Count - cell.Padding.Bottom
			};

			var isDrawNeeded = startLineIndex <= lineIndex && startLineIndex + cell.Content.Count > lineIndex;

			contentIndex = isDrawNeeded ? Math.Abs(startLineIndex - lineIndex) : default;

			return isDrawNeeded;
		}

		private static int GetStartIndexForCenteredContent(int contentAreaSize, int contentLength, int padding1, int padding2)
		{
			return Shared.Min(Shared.Max((contentAreaSize - contentLength) / 2, padding1), contentAreaSize - padding2 - contentLength);
		}

		private static void ChangeToCellContentColors(Cell cell)
		{
			Console.ForegroundColor = cell.IsSelected ?
				Settings.Current.SelectedCellForegroundColor : cell.ForegroundColor;
			Console.BackgroundColor = cell.IsSelected ?
				Settings.Current.SelectedCellBackgroundColor : cell.BackgroundColor;
		}

		private static void ChangeToCellBorderColors(Cell cell)
		{
			Console.ForegroundColor = cell.IsSelected ?
				Settings.Current.SelectedBorderForegroundColor : cell.BorderForegroundColor;
			Console.BackgroundColor = cell.IsSelected ?
				Settings.Current.SelectedBorderBackgroundColor : cell.BorderBackgroundColor;
		}

		private static void ChangeToTextColors()
		{
			Console.ForegroundColor = Settings.Current.TextForegroundColor;
			Console.BackgroundColor = Settings.Current.TextBackgroundColor;
		}

		//private static void CreateViewTable(Table table)
		//{
		//	_VIEW_TABLE = table;

		//var content = JsonConvert.SerializeObject(table, Formatting.Indented);
		//_VIEW_TABLE = JsonConvert.DeserializeObject<Table>(content);

		//var voStartPosition = _VIEW_TABLE.ViewOptions.StartPosition;
		//var voEndPosition = _VIEW_TABLE.ViewOptions.EndPosition;

		//var leftEllipsis = voStartPosition.X > 0;
		//var upEllipsis = voStartPosition.Y > 0;
		//var rightEllipsis = voEndPosition.X < table.Size.Width - 1;
		//var downEllipsis = voEndPosition.Y < table.Size.Height - 1;

		//_VIEW_TABLE.Cells = _VIEW_TABLE[voStartPosition.X, voStartPosition.Y, voEndPosition.X, voEndPosition.Y];
		//_VIEW_TABLE.Size = _VIEW_TABLE.ViewOptions.Size;

		//_VIEW_TABLE.AddRowAt(0);

		//for (int x = 0; x < _VIEW_TABLE.Size.Width; x++)
		//{
		//	var colIndex = _VIEW_TABLE.ViewOptions.StartPosition.X + x;

		//	_VIEW_TABLE[x, 0].ContentType = typeof(string);

		//	var left = leftEllipsis && x == 0 ? "0 ◀ " : "";
		//	var right = rightEllipsis && x == _VIEW_TABLE.Size.Width - 1 ? $" ▶ {table.Size.Width - 1}" : "";

		//	_VIEW_TABLE[x, 0].SetContent($"{left}{colIndex}{right}");

		//	if (table.IsColumnSelected(x + voStartPosition.X))
		//	{
		//		_VIEW_TABLE[x, 0].ForegroundColor = Settings.Current.SelectedCellForegroundColor;
		//	}
		//}

		//_VIEW_TABLE.AddColumnAt(0);

		//_VIEW_TABLE[0, 0].SetContent(@"y \ x");

		//for (int y = 1; y < _VIEW_TABLE.Size.Height; y++)
		//{
		//	var rowIndex = _VIEW_TABLE.ViewOptions.StartPosition.Y + y - 1;

		//	_VIEW_TABLE[0, y].ContentType = typeof(string);

		//	var left = upEllipsis && y == 1 ? "0 ▲ " : "";
		//	var rigth = downEllipsis && y == _VIEW_TABLE.Size.Height - 1 ? $" ▼ {table.Size.Height - 1}" : "";

		//	_VIEW_TABLE[0, y].SetContent($"{left}{rowIndex}{rigth}");

		//	if (table.IsRowSelected(y - 1 + voStartPosition.Y))
		//	{
		//		_VIEW_TABLE[0, y].ForegroundColor = Settings.Current.SelectedCellForegroundColor;
		//	}
		//}
		//}

		private static char GetTableCharacter(decimal borderRowNo, decimal borderColNo, int rowCount, int columnCount)
		{
			var betweenColumns = (int)borderColNo != borderColNo;
			var betweenRows = (int)borderRowNo != borderRowNo;

			if (!betweenColumns && !betweenRows)
			{
				var up = GetNextTableCharacter(borderRowNo > 0, borderColNo < 2, BorderType.Up);
				var left = GetNextTableCharacter(borderColNo > 0, borderRowNo < 2, BorderType.Left);
				var right = GetNextTableCharacter(borderColNo < columnCount, borderRowNo < 2, BorderType.Right);
				var down = GetNextTableCharacter(borderRowNo < rowCount, borderColNo < 2, BorderType.Down);

				return BorderCharacters.Get(up | left | right | down);
			}
			else if (betweenColumns)
			{
				return BorderCharacters.Get(GetNextTableCharacter(true, borderRowNo < 2, BorderType.Horizontal));
			}
			else if (betweenRows)
			{
				return BorderCharacters.Get(GetNextTableCharacter(true, borderColNo < 2, BorderType.Vertical));
			}

			throw new InvalidOperationException();
		}

		private static BorderType GetNextTableCharacter(bool caseSingle, bool caseDouble, BorderType value)
		{
			if (caseSingle && caseDouble)
			{
				return (BorderType)((int)value * 2);
			}
			else if (caseSingle)
			{
				return value;
			}

			return BorderType.None;
		}

		//private enum DrawColorSet
		//{
		//	Default,
		//	Border,
		//	SelectedBorder
		//}


	}
}
