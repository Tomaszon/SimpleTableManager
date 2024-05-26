using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public static class Renderer
{
	public static RendererSettings RendererSettings { get; set; } = new();

	private const int _FREE_LINES_BELOW_TABLE = 10;
	private const int _FREE_LINES_ABOW_TABLE = 15;

	public static void Render(Document document)
	{
		var table = document.GetActiveTable(out var tableIndex);

		var cells = table.GetSelectedCells();

		var singleCell = cells.Count() > 1 ? null : cells.FirstOrDefault();

		ChangeToTextColors();

		var tableSize = ShrinkTableViewToConsoleSize(table);

		var tableOffset = new Size((Console.WindowWidth - tableSize.Width) / 2, _FREE_LINES_ABOW_TABLE);

		RenderInfos(document, table, singleCell, tableOffset, tableIndex, document.Tables.Count);

		RenderTempCell(table, tableOffset, tableSize);

		RenderContent(table, tableOffset);

		RenderHeader(table, tableOffset);

		RenderSider(table, tableOffset);

		RenderCornerCell(table, tableOffset);

		Console.SetCursorPosition(0, Console.WindowHeight - _FREE_LINES_BELOW_TABLE);

		ChangeToTextColors();
	}

	private static Size ShrinkTableViewToConsoleSize(Table table)
	{
		while (true)
		{
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
		var placeHolderCell = new Cell(table, GetTmpBackground(size)) { ContentColor = new(Settings.Current.TextColor) };
		var placeHolderCellPosition = new Position(tableOffset);

		DrawCellBorders(placeHolderCell, placeHolderCellPosition, size, CellBorders.Get(CellBorderType.CornerCellClosed));

		DrawCellContent(placeHolderCell, placeHolderCellPosition, size, true, false);

		Task.Delay(500).Wait();
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

		DrawCellContent(table.CornerCell, position, size, true, false);
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

				var border = CellBorders.Get(GetHeaderCellBorderType(table.ViewOptions.Size.Width, posInView.X));

				DrawCellBorders(cell, position, size, border);

				DrawCellContent(cell, position, size, true, table.IsColumnSelected(x));
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

				var border = CellBorders.Get(GetSiderCellBorderType(table.ViewOptions.Size.Height, posInView.Y));

				DrawCellBorders(cell, position, size, border);

				DrawCellContent(cell, position, size, true, table.IsRowSelected(y));
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

					DrawCellContent(cell, position, size, false, false);
				}
			}));
	}

	private static CellBorder GetContentCellBorder(Table table, Cell cell, Position position)
	{
		var cellBorderType = GetContentCellBorderType(table.ViewOptions.Size, position);

		var border = CellBorders.Get(cellBorderType);

		return TrimContentCellBorder(border, table, cell, position);
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

	public static Cell? GetNearestVisibleCell(Table table, Position position, int horizontalIncrement, int verticalIncrement)
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

	private static void DrawCellContent(Cell cell, Position position, Size size, bool ignoreRenderingMode, bool showSelection)
	{
		var sizeWithoutBorders = new Size(size.Width - 2, size.Height - 2);

		var contentsToRender = cell.GetFormattedContents();
		var hAlignment = cell.ContentAlignment.Horizontal;
		var vAlignment = cell.ContentAlignment.Vertical;

		if (!ignoreRenderingMode)
		{
			switch (RendererSettings.RenderingMode)
			{
				case RenderingMode.Layer:
					{
						contentsToRender = new List<string>() { cell.LayerIndex.ToString() };
						hAlignment = HorizontalAlignment.Center;
						vAlignment = VerticalAlignment.Center;
					}
					break;

				case RenderingMode.Comment:
					{
						contentsToRender = cell.Comment?.Wrap() ?? Enumerable.Empty<string>();
						hAlignment = HorizontalAlignment.Center;
						vAlignment = VerticalAlignment.Center;
					}
					break;
			}

		}

		Shared.IndexArray(sizeWithoutBorders.Height).ForEach(i =>
		{
			Console.SetCursorPosition(position.X + 1, position.Y + 1 + i);

			var content = "";
			var leftPaddingSize = sizeWithoutBorders.Width;
			var rightPaddingSize = 0;

			if (IsCellContentDrawNeeded(contentsToRender, vAlignment, cell.ContentPadding, i, sizeWithoutBorders.Height, out var contentIndex) && contentsToRender.Count() > contentIndex)
			{
				content = contentsToRender.ElementAt(contentIndex);

				if (!string.IsNullOrWhiteSpace(content))
				{
					switch (hAlignment)
					{
						case HorizontalAlignment.Left:
							{
								leftPaddingSize = cell.ContentPadding.Left;
								rightPaddingSize = content.Length + leftPaddingSize > sizeWithoutBorders.Width ? 0 : sizeWithoutBorders.Width - content.Length - leftPaddingSize;
							}
							break;

						case HorizontalAlignment.Center:
							{
								var startIndex = GetStartIndexForCenteredContent(sizeWithoutBorders.Width, content.Length, cell.ContentPadding.Left, cell.ContentPadding.Right);

								leftPaddingSize = startIndex;
								rightPaddingSize = content.Length + leftPaddingSize > sizeWithoutBorders.Width ? 0 : sizeWithoutBorders.Width - content.Length - leftPaddingSize;
							}
							break;

						case HorizontalAlignment.Right:
							{
								rightPaddingSize = cell.ContentPadding.Right;
								leftPaddingSize = content.Length + rightPaddingSize > sizeWithoutBorders.Width ? 0 : sizeWithoutBorders.Width - content.Length - rightPaddingSize;
							}
							break;
					}
				}
			}

			ChangeToCommentContentColors(cell);
			ShowIndexCellSelection(showSelection);
			Console.Write(new string(Settings.Current.CellBackgroundCharacter, leftPaddingSize));

			ChangeToCellContentColors(cell);

			if (!ignoreRenderingMode)
			{
				switch (RendererSettings.RenderingMode)
				{
					case RenderingMode.Layer:
						ChangeToLayerIndexContentColors(cell);
						break;

					case RenderingMode.Comment:
						ChangeToCommentContentColors(cell);
						break;
				}
			}

			ShowIndexCellSelection(showSelection);
			Console.Write(content);

			ChangeToCommentContentColors(cell);
			ShowIndexCellSelection(showSelection);
			Console.Write(new string(Settings.Current.CellBackgroundCharacter, rightPaddingSize));
		});
	}

	private static void RenderInfos(Document document, Table table, Cell? cell, Size tableOffset, int tableIndex, int tableCount)
	{
		//IDEA
		Console.SetCursorPosition(Console.WindowWidth / 2 - 8, 0);
		Console.Write(" ____ _____ _   _");
		Console.SetCursorPosition(Console.WindowWidth / 2 - 8, 1);
		Console.Write("/ ___|__ __| \\ / |");
		Console.SetCursorPosition(Console.WindowWidth / 2 - 8, 2);
		Console.Write("\\___ \\ | | |  v  |");
		Console.SetCursorPosition(Console.WindowWidth / 2 - 8, 3);
		Console.Write("|____/ |_| |_| |_|");

		var infoTable = new Table("", 2, 3);
		infoTable[0, 0].SetContent("Title:");
		infoTable[1, 0].SetContent($"{document.Metadata.Title} by {document.Metadata.Author}{(document.IsSaved is null ? "" : document.IsSaved == true ? Settings.Current.Autosave ? " - (Autosaved)" : " - (Saved)" : " - (Unsaved)")}");
		infoTable[0, 1].SetContent("Created:", "Size:");
		infoTable[1, 1].SetContent(document.Metadata.CreateTime is not null ? $"{document.Metadata.CreateTime}" : "Not saved yet", document.Metadata.Size is not null ? $"{document.Metadata.Size} bytes" : "Not saved yet");

		infoTable[0, 2].SetContent("Path:");
		infoTable[1, 2].SetContent(document.Metadata.Path is not null ? document.Metadata.Path : "Not saved yet");

		infoTable.Content.ForEach(cell =>
			cell.SetHorizontalAlignment(HorizontalAlignment.Left));

		RenderContent(infoTable, new((Console.WindowWidth - infoTable.GetTableSize().Width - infoTable.GetSiderWidth()) / 2, 3));

		// //IDEA
		// Console.Write("Cell:");
		// if (cell is null)
		// {
		// 	Console.WriteLine("Select one cell to show details");
		// }
		// else
		// {
		// 	//EXPERIMENTAL dynamic type handling
		// 	dynamic details = cell.ShowDetails();
		// 	Console.Write($"Function:    {details.Content.Function}    Comment:    {details.Comment}    Layer index: {details.LayerIndex}");
		// }

		ChangeToTextColors();

		Console.SetCursorPosition(tableOffset.Width, tableOffset.Height - 1);
		Console.Write($"Table:    {table.Name}    ");

		if (tableIndex > 0 && tableIndex < tableCount - 1)
		{
			Console.WriteLine($"0 {Settings.Current.IndexCellLeftArrow} {tableIndex} {Settings.Current.IndexCellRightArrow} {tableCount - 1}");
		}
		else if (tableIndex == 0 && tableIndex < tableCount - 1)
		{
			Console.WriteLine($"{tableIndex} {Settings.Current.IndexCellRightArrow} {tableCount - 1}");
		}
		else if (tableIndex > 0 && tableIndex == tableCount - 1)
		{
			Console.WriteLine($"0 {Settings.Current.IndexCellLeftArrow} {tableIndex}");
		}
		else
		{
			Console.WriteLine();
		}
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

	public static void ShowIndexCellSelection(bool selected)
	{
		if (selected)
		{
			Console.ForegroundColor = Settings.Current.SelectedContentColor.Foreground;
			Console.BackgroundColor = Settings.Current.SelectedContentColor.Background;
		}
	}

	private static void ChangeToCommentContentColors(Cell cell)
	{
		Console.ForegroundColor = cell.IsSelected ?
			Settings.Current.SelectedContentColor.Foreground :
			Settings.Current.NotAvailableContentColor.Foreground;
		Console.BackgroundColor = cell.IsSelected ?
			Settings.Current.SelectedContentColor.Background :
			Settings.Current.NotAvailableContentColor.Background;
	}

	private static void ChangeToLayerIndexContentColors(Cell cell)
	{
		Console.ForegroundColor = cell.IsSelected ?
			Settings.Current.SelectedContentColor.Foreground :
				cell.LayerIndex == 0 ?
					Settings.Current.NotAvailableContentColor.Foreground :
						cell.ContentColor.Foreground;
		Console.BackgroundColor = cell.IsSelected ?
			Settings.Current.SelectedContentColor.Background :
				cell.LayerIndex == 0 ?
					Settings.Current.NotAvailableContentColor.Background :
						cell.ContentColor.Background;
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

	private static void ChangeToOkLabelColors()
	{
		Console.ForegroundColor = Settings.Current.OkLabelColor.Foreground;
		Console.BackgroundColor = Settings.Current.OkLabelColor.Background;
	}

	private static void ChangeToNotOkLabelColors()
	{
		Console.ForegroundColor = Settings.Current.NotOkLabelColor.Foreground;
		Console.BackgroundColor = Settings.Current.NotOkLabelColor.Background;
	}

	public static void ChangeToDefaultBorderColors()
	{
		Console.ForegroundColor = Settings.Current.DefaultBorderColor.Foreground;
		Console.BackgroundColor = Settings.Current.DefaultBorderColor.Background;
	}
}