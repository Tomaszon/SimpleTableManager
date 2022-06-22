using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class Renderer
	{
		private static int _FREE_LINES_BELOW_TABLE = 16;
		private static int _FREE_LINES_ABOW_TABLE = 10;

		public static void Render(Document document)
		{
			var table = document.GetActiveTable();

			ChangeToTextColors();

			//Console.WriteLine($"{table.Name}\n");

			var tableSize = MaximizeTableView(table);

			var tableOffset = new Size((Console.WindowWidth - tableSize.Width) / 2, _FREE_LINES_ABOW_TABLE);

			RenderInfos(document.Metadata, table, tableOffset);

			RenderTempCell(tableOffset, tableSize);

			RenderCornerCell(table, tableOffset);

			RenderHeader(table, tableOffset);

			RenderSider(table, tableOffset);

			RenderContent(table, tableOffset);

			Console.SetCursorPosition(0, Console.WindowHeight - _FREE_LINES_BELOW_TABLE);

			ChangeToTextColors();
		}

		private static void SetEllipsesToIndexCells(List<IndexCell> collection, IndexCell firstCell, IndexCell lastCell, int size)
		{
			collection.ForEach(s => s.Normalize());

			if (firstCell is not null && firstCell.Index > 0)
			{
				firstCell.AppendLowerEllipsis();
			}
			if (lastCell is not null && lastCell.Index < size - 1)
			{
				lastCell.AppendHigherEllipsis(size);
			}
		}

		private static Size MaximizeTableView(Table table)
		{
			while (true)
			{
				SetEllipsesToIndexCells(table.Sider, table.GetFirstVisibleSiderInView(), table.GetLastVisibleSiderInView(), table.Size.Height);

				SetEllipsesToIndexCells(table.Header, table.GetFirstVisibleHeaderInView(), table.GetLastVisibleHeaderInView(), table.Size.Width);

				var size = table.GetTableSize();

				if (Console.WindowWidth - size.Width < 0)
				{
					table.ViewOptions.DecreaseWidth();
				}
				else if (Console.WindowHeight - _FREE_LINES_BELOW_TABLE - _FREE_LINES_ABOW_TABLE - size.Height < 0)
				{
					table.ViewOptions.DecreaseHeight();
				}
				else
				{
					return size;
				}
			}
		}

		private static void RenderTempCell(Size tableOffset, Size size)
		{
			var placeHolderCell = new Cell(GetTmpBackground(size)) { ContentColor = new ConsoleColorSet(Settings.Current.TextColor) };
			var placeHolderCellPosition = new Position(tableOffset);

			DrawCellBorder(placeHolderCell, placeHolderCellPosition, size, CellBorders.Get(CellBorderType.CornerCellClosed));

			DrawCellContent(placeHolderCell, placeHolderCellPosition, size);

			Task.Delay(250).Wait();
		}

		private static string[] GetTmpBackground(Size size)
		{
			var pattern1 = $" {Settings.Current.TmpBackgroundCharacter}  ";
			var pattern2 = $"   {Settings.Current.TmpBackgroundCharacter}";

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

			DrawCellBorder(table.CornerCell, position, size, CellBorders.Get(CellBorderType.CornerCellOpen));

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

					var posInView = table.PositionInView(x, -1);

					cell.ShowSelection(table.IsColumnSelected(x));

					var border = CellBorders.Get(GetHeaderCellBorderType(table.ViewOptions.Size.Width, posInView.X));

					DrawCellBorder(cell, position, size, border);

					DrawCellContent(cell, position, size);
				}
			});
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

					var posInView = table.PositionInView(-1, y);

					cell.ShowSelection(table.IsRowSelected(y));

					var border = CellBorders.Get(GetSiderCellBorderType(table.ViewOptions.Size.Height, posInView.Y));

					DrawCellBorder(cell, position, size, border);

					DrawCellContent(cell, position, size);
				}
			});
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

						var posInView = table.PositionInView(x, y);

						var border = GetContentCellBorder(table, cell, posInView);

						DrawCellBorder(cell, position, size, border);

						DrawCellContent(cell, position, size);
					}
				}));
		}

		private static CellBorder GetContentCellBorder(Table table, Cell cell, Position position)
		{
			var cellBorderType = GetContentCellBorderType(table.ViewOptions.Size, position);

			var border = CellBorders.Get(cellBorderType);

			border = DotContentCellBorder(border, table, cell, position);

			return TrimContentCellBorder(border, table, cell, position);
		}

		private static CellBorder DotContentCellBorder(CellBorder border, Table table, Cell cell, Position position)
		{
			//TODO
			return border;
		}

		private static CellBorder TrimContentCellBorder(CellBorder border, Table table, Cell cell, Position position)
		{
			if (position.X == 0 || position.X > 0 && table[position.X - 1, position.Y].IsSelected && !cell.IsSelected)
			{
				border = border.Trim(left: true);
			}
			if (position.Y == 0 || position.Y > 0 && table[position.X, position.Y - 1].IsSelected && !cell.IsSelected)
			{
				border = border.Trim(top: true);
			}
			if (position.X < table.Size.Width - 1 && table[position.X + 1, position.Y].IsSelected && !cell.IsSelected)
			{
				border = border.Trim(right: true);
			}
			if (position.Y < table.Size.Height - 1 && table[position.X, position.Y + 1].IsSelected && !cell.IsSelected)
			{
				border = border.Trim(bottom: true);
			}

			return border;
		}

		private static CellBorderType GetContentCellBorderType(Size size, Position position)
		{
			if (position.X == size.Width - 1 && position.Y == size.Height - 1)
			{
				return CellBorderType.ContentUpLeft;
			}
			else if (position.X == size.Width - 1)
			{
				return CellBorderType.ContentVerticalLeft;
			}
			else if (position.Y == size.Height - 1)
			{
				return CellBorderType.ContentHorizontalUp;
			}
			else
			{
				return CellBorderType.ContentOpen;
			}
		}

		private static CellBorderType GetHeaderCellBorderType(int size, int position)
		{
			if (position == size - 1)
			{
				return CellBorderType.HeaderVertical;
			}
			else
			{
				return CellBorderType.HeaderOpen;
			}
		}

		private static CellBorderType GetSiderCellBorderType(int size, int position)
		{
			if (position == size - 1)
			{
				return CellBorderType.SiderHorizontal;
			}
			else
			{
				return CellBorderType.SiderOpen;
			}
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
						switch (cell.ContentAlignment.Horizontal)
						{
							case HorizontalAlignment.Left:
								{
									content = content.AppendLeft(' ', cell.ContentPadding.Left);
									content = content.PadRight(sizeWithoutBorders.Width);
								}
								break;

							case HorizontalAlignment.Center:
								{
									var startIndex = GetStartIndexForCenteredContent(sizeWithoutBorders.Width, content.Length, cell.ContentPadding.Left, cell.ContentPadding.Right);

									content = content.AppendLeft(' ', startIndex);
									content = content.PadRight(sizeWithoutBorders.Width);
								}
								break;

							case HorizontalAlignment.Right:
								{
									content = content.AppendRight(' ', cell.ContentPadding.Right);
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

		private static void RenderInfos(Metadata metadata, Table table, Size tableOffset)
		{
			ChangeToTextColors();

			Console.SetCursorPosition(tableOffset.Width + 1, tableOffset.Height - 4);
			Console.WriteLine($"Document title: {metadata.Title}");

			Console.SetCursorPosition(tableOffset.Width + 1, tableOffset.Height - 3);
			Console.WriteLine(metadata.Path is not null ? $"Document path: {metadata.Path}" : "Unsaved document");

			Console.SetCursorPosition(tableOffset.Width + 1, tableOffset.Height - 1);
			Console.WriteLine($"Table name: {table.Name}");
		}

		private static bool IsCellContentDrawNeeded(Cell cell, int lineIndex, int height, out int contentIndex)
		{
			var startLineIndex = cell.ContentAlignment.Vertical switch
			{
				VerticalAlignment.Top => cell.ContentPadding.Top,
				VerticalAlignment.Center => GetStartIndexForCenteredContent(height, cell.Content.Count, cell.ContentPadding.Top, cell.ContentPadding.Bottom),
				_ => height - cell.Content.Count - cell.ContentPadding.Bottom
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
				Settings.Current.SelectedContentColor.Foreground : cell.ContentColor.Foreground;
			Console.BackgroundColor = cell.IsSelected ?
				Settings.Current.SelectedContentColor.Background : cell.ContentColor.Background;
		}

		private static void ChangeToCellBorderColors(Cell cell)
		{
			Console.ForegroundColor = cell.IsSelected ?
				Settings.Current.SelectedBorderColor.Foreground : cell.BorderColor.Foreground;
			Console.BackgroundColor = cell.IsSelected ?
				Settings.Current.SelectedBorderColor.Background : cell.BorderColor.Background;
		}

		private static void ChangeToTextColors()
		{
			Console.ForegroundColor = Settings.Current.TextColor.Foreground;
			Console.BackgroundColor = Settings.Current.TextColor.Background;
		}
	}
}
