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
		public static RendererSettings RendererSettings { get; set; } = new RendererSettings();

		private const int _FREE_LINES_BELOW_TABLE = 16;
		private const int _FREE_LINES_ABOW_TABLE = 10;

		public static void Render(Document document)
		{
			var table = document.GetActiveTable();

			ChangeToTextColors();

			var tableSize = MaximizeTableView(table);

			var tableOffset = new Size((Console.WindowWidth - tableSize.Width) / 2, _FREE_LINES_ABOW_TABLE);

			RenderInfos(document.Metadata, table, tableOffset);

			RenderTempCell(table, tableOffset, tableSize);

			RenderContent(table, tableOffset);

			RenderHeader(table, tableOffset);

			RenderSider(table, tableOffset);

			RenderCornerCell(table, tableOffset);

			Console.SetCursorPosition(0, Console.WindowHeight - _FREE_LINES_BELOW_TABLE);

			ChangeToTextColors();
		}

		private static void SetEllipsesToIndexCells(List<IndexCell> collection, IndexCell firstCell, IndexCell lastCell, int size)
		{
			collection.ForEach(s => s.RemoveEllipses());

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

		private static void RenderTempCell(Table table, Size tableOffset, Size size)
		{
			var placeHolderCell = new Cell(table, GetTmpBackground(size)) { ContentColor = new ConsoleColorSet(Settings.Current.TextColor) };
			var placeHolderCellPosition = new Position(tableOffset);

			DrawCellBorders(placeHolderCell, placeHolderCellPosition, size, CellBorders.Get(CellBorderType.CornerCellClosed));

			DrawCellContent(placeHolderCell, placeHolderCellPosition, size, true);

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
		}

		private static string GetLine(Size size, string value)
		{
			return Shared.IndexArray((int)Math.Ceiling((size.Width - 2) / (decimal)value.Length))
				.Aggregate("", (s, i) => s += value)[..(size.Width - 2)];
		}

		private static void RenderCornerCell(Table table, Size tableOffset)
		{
			var position = new Position(tableOffset.Width, tableOffset.Height);

			var size = table.GetCornerCellSize();

			DrawCellBorders(table.CornerCell, position, size, CellBorders.Get(CellBorderType.CornerCellOpen));

			DrawCellContent(table.CornerCell, position, size, true);
		}

		private static void RenderHeader(Table table, Size tableOffset)
		{
			Shared.IndexArray(table.Size.Width).ForEach(x =>
			{
				var cell = table.Header[x];

				if (cell.Visibility.IsVisible && table.IsCellInView(x: x))
				{
					var position = table.GetHeaderCellPosition(tableOffset, x);

					var size = table.GetHeaderCellSize(x);

					var posInView = table.PositionInView(x, -1);

					//todo rework, remove from cell
					cell.ShowSelection(table.IsColumnSelected(x));

					var border = CellBorders.Get(GetHeaderCellBorderType(table.ViewOptions.Size.Width, posInView.X));

					DrawCellBorders(cell, position, size, border);

					DrawCellContent(cell, position, size, true);
				}
			});
		}

		private static void RenderSider(Table table, Size tableOffset)
		{
			Shared.IndexArray(table.Size.Height).ForEach(y =>
			{
				var cell = table.Sider[y];
				if (cell.Visibility.IsVisible && table.IsCellInView(y: y))
				{
					var position = table.GetSiderCellPosition(tableOffset, y);

					var size = table.GetSiderCellSize(y);

					var posInView = table.PositionInView(-1, y);

					cell.ShowSelection(table.IsRowSelected(y));

					var border = CellBorders.Get(GetSiderCellBorderType(table.ViewOptions.Size.Height, posInView.Y));

					DrawCellBorders(cell, position, size, border);

					DrawCellContent(cell, position, size, true);
				}
			});
		}

		private static void RenderContent(Table table, Size tableOffset)
		{
			Shared.IndexArray(table.Size.Height).ForEach(y =>
				Shared.IndexArray(table.Size.Width).ForEach(x =>
				{
					var cell = table[x, y];

					if (cell.Visibility.IsVisible && table.IsCellInView(x, y))
					{
						var position = table.GetContentCellPosition(tableOffset, x, y);

						var size = table.GetContentCellSize(x, y);

						var posInView = table.PositionInView(x, y);

						var border = GetContentCellBorder(table, cell, posInView);

						DrawCellBorders(cell, position, size, border);

						DrawCellContent(cell, position, size, false);
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
			var matrix = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 }, { 1, 1 } };

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				if (GetNearestVisibleCell(table, position, matrix[i, 0], matrix[i, 1]) is var nearestCell && nearestCell is not null &&
					(nearestCell.IsSelected && !cell.IsSelected || nearestCell.LayerIndex > cell.LayerIndex && !cell.IsSelected))
				{
					if (matrix[i, 0] == 0 || matrix[i, 1] == 0)
					{
						border = border.TrimSide(matrix[i, 1] == -1, matrix[i, 1] == 1, matrix[i, 0] == -1, matrix[i, 0] == 1);
					}
					else
					{
						border = border.TrimCorner(matrix[i, 0] == -1 && matrix[i, 1] == -1, matrix[i, 0] == 1 && matrix[i, 1] == -1,
							matrix[i, 0] == -1 && matrix[i, 1] == 1, matrix[i, 0] == 1 && matrix[i, 1] == 1);
					}
				}
			};

			return border;
		}

		public static Cell GetNearestVisibleCell(Table table, Position position, int horizontalIncrement, int verticalIncrement)
		{
			var y = position.Y;
			var x = position.X;

			while (x + horizontalIncrement >= 0 && x + horizontalIncrement <= table.Size.Width - 1 &&
				y + verticalIncrement >= 0 && y + verticalIncrement <= table.Size.Height - 1)
			{
				y += verticalIncrement;
				x += horizontalIncrement;

				var cell = table[x, y];

				if (cell.Visibility.IsVisible)
				{
					return cell;
				}
			}

			return null;
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

		private static void DrawCellBorders(Cell cell, Position position, Size size, CellBorder border)
		{
			ChangeToCellBorderColors(cell);

			if (border.Top != BorderType.None)
			{
				Console.SetCursorPosition(position.X + 1, position.Y);
				DrawBorderSegment(border.Top, size.Width - 2);
			}

			if (border.Left != BorderType.None)
			{
				Console.SetCursorPosition(position.X, position.Y + 1);
				Shared.IndexArray(size.Height - 2, 1).ForEach(i =>
				{
					DrawBorderSegment(border.Left);
					Shared.StepCursor(-1, 1);
				});
			}

			if (border.Right != BorderType.None)
			{
				Console.SetCursorPosition(position.X + size.Width - 1, position.Y + 1);
				Shared.IndexArray(size.Height - 2, 1).ForEach(i =>
				{
					DrawBorderSegment(border.Right);
					Shared.StepCursor(-1, 1);
				});
			}

			if (border.Bottom != BorderType.None)
			{
				Console.SetCursorPosition(position.X + 1, position.Y + size.Height - 1);
				DrawBorderSegment(border.Bottom, size.Width - 2);
			}

			if (border.TopLeft != BorderType.None)
			{
				Console.SetCursorPosition(position.X, position.Y);
				DrawBorderSegment(border.TopLeft);
			}

			if (border.TopRight != BorderType.None)
			{
				Console.SetCursorPosition(position.X + size.Width - 1, position.Y);
				DrawBorderSegment(border.TopRight);
			}

			if (border.BottomLeft != BorderType.None)
			{
				Console.SetCursorPosition(position.X, position.Y + size.Height - 1);
				DrawBorderSegment(border.BottomLeft);
			}

			if (border.BottomRight != BorderType.None)
			{
				Console.SetCursorPosition(position.X + size.Width - 1, position.Y + size.Height - 1);
				DrawBorderSegment(border.BottomRight);
			}
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

		private static void DrawCellContent(Cell cell, Position position, Size size, bool ignoreRenderingMode)
		{
			var sizeWithoutBorders = new Size(size.Width - 2, size.Height - 2);

			var contentToRender = cell.GetContents();
			var horizontalAlignmentToRender = cell.ContentAlignment.Horizontal;
			var verticalAlignmentToRender = cell.ContentAlignment.Vertical;

			if (!ignoreRenderingMode && RendererSettings.RenderingMode == RenderingMode.Layer)
			{
				contentToRender = cell.LayerIndex == 0 ? new List<object>() : new List<object>() { cell.LayerIndex };
				horizontalAlignmentToRender = HorizontalAlignment.Center;
				verticalAlignmentToRender = VerticalAlignment.Center;
			}

			Shared.IndexArray(sizeWithoutBorders.Height).ForEach(i =>
			{
				Console.SetCursorPosition(position.X + 1, position.Y + 1 + i);

				var content = new string(' ', sizeWithoutBorders.Width);

				if (IsCellContentDrawNeeded(contentToRender, verticalAlignmentToRender, cell.ContentPadding, i, sizeWithoutBorders.Height, out var contentIndex) && contentToRender.Count() > contentIndex)
				{
					content = contentToRender.ElementAt(contentIndex).ToString();

					if (!string.IsNullOrWhiteSpace(content))
					{
						switch (horizontalAlignmentToRender)
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

		private static bool IsCellContentDrawNeeded(IEnumerable<object> contents, VerticalAlignment alignment, ContentPadding padding, int lineIndex, int height, out int contentIndex)
		{
			var startLineIndex = alignment switch
			{
				VerticalAlignment.Top => padding.Top,
				VerticalAlignment.Center => GetStartIndexForCenteredContent(height, contents.Count(), padding.Top, padding.Bottom),
				_ => height - contents.Count() - padding.Bottom
			};

			var isDrawNeeded = startLineIndex <= lineIndex && startLineIndex + contents.Count() > lineIndex;

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

		public static void ChangeToDefaultBorderColors()
		{
			Console.ForegroundColor = Settings.Current.DefaultBorderColor.Foreground;
			Console.BackgroundColor = Settings.Current.DefaultBorderColor.Background;
		}
	}
}
