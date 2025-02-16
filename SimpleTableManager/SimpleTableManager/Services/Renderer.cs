namespace SimpleTableManager.Services;

[ExcludeFromCodeCoverage]
public static class Renderer
{
	public static RendererSettings RendererSettings { get; set; } = new();

	private const int _FREE_LINES = 10;

	private const int _MINIMUM_LINES_FOR_LOGO = 50;

	private const int _MINIMUM_LINES_FOR_DOCUMENT_INFOS = 45;

	private const int _MINIMUM_LINES_FOR_FUNCTION_INFOS = 40;

	private const int _MINIMUM_COLUMNS_FOR_CELL_INFOS = 100;

	private const int _TABLE_GAP = 2;

	public static void Render(Document document)
	{
		var table = document.GetActiveTable(out var tableIndex);

		var selectedCells = table.GetPrimarySelectedCells();

		ChangeToTextColors();

		RenderLogo(out var logoBottomOffset);

		RenderDocumentInfos(document, logoBottomOffset, out var documentInfosBottomOffset);

		RenderCellInfos([.. selectedCells], table, out var cellInfosLeftOffset);

		RenderFunctionInfos([.. selectedCells], documentInfosBottomOffset, out var functionInfosBottomOffset);

		var tableSize = ShrinkTableViewToConsoleSize(table, functionInfosBottomOffset + _TABLE_GAP, Console.WindowWidth - cellInfosLeftOffset + _TABLE_GAP);

		var tableOffset = tableSize.Width < Console.WindowWidth - (Console.WindowWidth - cellInfosLeftOffset) * 2 - _TABLE_GAP ?
			new Size((Console.WindowWidth - tableSize.Width) / 2, (Console.WindowHeight - _FREE_LINES - functionInfosBottomOffset - _TABLE_GAP - tableSize.Height) / 2 + functionInfosBottomOffset + _TABLE_GAP) :
			new Size((cellInfosLeftOffset - _TABLE_GAP - tableSize.Width) / 2, (Console.WindowHeight - _FREE_LINES - functionInfosBottomOffset - _TABLE_GAP - tableSize.Height) / 2 + functionInfosBottomOffset + _TABLE_GAP);

		RenderTableInfos(table, new(tableOffset.Width, tableOffset.Height - (_TABLE_GAP / 2)), tableIndex, document.Tables.Count);

		RenderTempCell(table, tableOffset, tableSize);

		RenderContent(table, tableOffset, false);

		RenderHeader(table, tableOffset);

		RenderSider(table, tableOffset);

		RenderCornerCell(table, tableOffset);

		Console.SetCursorPosition(0, Console.WindowHeight - _FREE_LINES);

		ChangeToTextColors();
	}

	private static void RenderFunctionInfos(List<Cell> cells, int verticalOffset, out int bottomOffset)
	{
		if (Console.WindowHeight > _MINIMUM_LINES_FOR_FUNCTION_INFOS)
		{
			var infoTable = new Table(null!, "", 2, 1)
			{
				IsHeadLess = true
			};

			int cellMaxWidth = Console.WindowWidth - 20;
			var cellContents = cells.Count == 0 ? null :
				(cells.Count == 1 ?
					cells.First().ContentFunction?.ToString() :
					cells.All(c => c.ContentFunction is null) ?
						null :
						"Multiple\n")?
				.Split('\n');
			var cellMinWidth = Console.WindowWidth / 3;
			var cellContentLength = cellContents?.Max(e => e.Length) ?? 0;

			infoTable[0, 0].SetStringContent("Fn:");
			infoTable[1, 0].SetStringContent(
				cellContentLength > cellMaxWidth ?
					$"{cellContents?[0]![..cellMaxWidth]} ..." :
					cellContents?[0] ?? "None",
				cellContentLength > cellMaxWidth ?
					$"{cellContents?[1]![..cellMaxWidth]} ..." :
					cellContents?[1] ?? "");
			infoTable[1, 0].GivenSize = new Size(cellMinWidth, 1);

			infoTable.Content.ForEach(cell =>
			{
				cell.SetHorizontalAlignment(HorizontalAlignment.Left);
				cell.BackgroundCharacter = ' ';
			});

			var tableSize = infoTable.GetTableSize();

			RenderContent(infoTable, new((Console.WindowWidth - tableSize.Width) / 2, verticalOffset + _TABLE_GAP), true);

			bottomOffset = verticalOffset + _TABLE_GAP + infoTable.GetTableSize().Height;
		}
		else
		{
			bottomOffset = verticalOffset;
		}
	}

	private static void RenderCellInfos(List<Cell> cells, Table table, out int leftOffset)
	{
		if (Console.WindowWidth > _MINIMUM_COLUMNS_FOR_CELL_INFOS && cells.Count > 0)
		{
			var outTypes = cells.Select(c => c.ContentFunction?.GetOutType().GetFriendlyName()).Where(t => t is not null).Cast<string>().Distinct().ToList();

			var layerIndices = cells.Select(c => c.LayerIndex).Distinct();

			var comments = cells.Select(c => c.Comments).Where(c => c.Count > 0).ToList();

			var position = cells.Count == 1 ? table[cells.Single()].ToString() : "Multiple";

			var outType = outTypes.Count == 0 ? " - " : outTypes.Count == 1 ? outTypes.Single() : "Multiple";

			var layerIndex = layerIndices.Count() == 1 ? layerIndices.Single().ToString() : "Multiple";

			var comment = comments.Count == 0 ? " - ".Wrap() : comments.Count == 1 ? comments.Single() : "Multiple".Wrap();

			var infoTable = new Table(null!, "", 2, 5)
			{
				IsHeadLess = true
			};

			infoTable[0, 0].SetStringContent("Selected:");
			infoTable[1, 0].SetStringContent(cells.Count.ToString());
			infoTable[0, 1].SetStringContent("Position:");
			infoTable[1, 1].SetStringContent(position);
			infoTable[0, 2].SetStringContent("Content:");
			infoTable[1, 2].SetStringContent(outType);
			infoTable[0, 3].SetStringContent("Layer:");
			infoTable[1, 3].SetStringContent(layerIndex);
			infoTable[0, 4].SetStringContent("Comment:");
			infoTable[1, 4].SetStringContent(comment.SelectMany(c => c.Chunk(15)).Select(c => new string(c)));

			infoTable.Content.ForEach(cell =>
			{
				cell.SetHorizontalAlignment(HorizontalAlignment.Left);
				cell.BackgroundCharacter = ' ';
			});

			var tableSize = infoTable.GetTableSize();

			leftOffset = Console.WindowWidth - tableSize.Width - _TABLE_GAP;

			RenderContent(infoTable, new(leftOffset, (Console.WindowHeight - tableSize.Height) / 2), true);
		}
		else
		{
			leftOffset = Console.WindowWidth + 1;
		}
	}

	private static void RenderLogo(out int bottomOffset)
	{
		if (Console.WindowHeight > _MINIMUM_LINES_FOR_LOGO)
		{
			var version = Shared.GetAppVersion();

			var maxLength = Settings.Current.Logo.Max(s => s.Replace("{version}", "").Length);

			for (int i = 0; i < Settings.Current.Logo.Length; i++)
			{
				Console.SetCursorPosition((Console.WindowWidth - maxLength) / 2, i);
				Console.Write(Settings.Current.Logo[i].Replace("{version}", $"v.{version}"));
			}

			bottomOffset = Settings.Current.Logo.Length - 1;
		}
		else
		{
			bottomOffset = -2;
		}
	}

	private static void RenderDocumentInfos(Document document, int verticalOffset, out int bottomOffset)
	{
		if (Console.WindowHeight > _MINIMUM_LINES_FOR_DOCUMENT_INFOS)
		{
			var title = $"{document.Metadata.Title} by {document.Metadata.Author}{(document.IsSaved is null ? "" : document.IsSaved == true ? Settings.Current.AutoSave ? " - (Autosaved)" : " - (Saved)" : " - (Unsaved)")}";
			var createTime = document.Metadata.CreateTime is not null ? $"{document.Metadata.CreateTime}" : "Not saved yet";
			var size = document.Metadata.Size is not null ? $"{document.Metadata.Size} bytes" : "Not saved yet";
			var path = document.Metadata.Path is not null ? document.Metadata.Path : "Not saved yet";

			var infoTable = new Table(null!, "", 2, 3)
			{
				IsHeadLess = true
			};

			infoTable[0, 0].SetStringContent("Title:");
			infoTable[1, 0].SetStringContent(title);
			infoTable[0, 1].SetStringContent("Created:", "Size:");
			infoTable[1, 1].SetStringContent(createTime, size);
			infoTable[0, 2].SetStringContent("Path:");
			infoTable[1, 2].SetStringContent(path);

			infoTable.Content.ForEach(cell =>
			{
				cell.SetHorizontalAlignment(HorizontalAlignment.Left);
				cell.BackgroundCharacter = ' ';
			});

			var tableSize = infoTable.GetTableSize();

			RenderContent(infoTable, new((Console.WindowWidth - tableSize.Width) / 2, verticalOffset + _TABLE_GAP), true);

			bottomOffset = verticalOffset + _TABLE_GAP + infoTable.GetTableSize().Height;
		}
		else
		{
			bottomOffset = verticalOffset;
		}
	}

	private static void RenderTableInfos(Table table, Size offset, int tableIndex, int tableCount)
	{
		if (offset.Height >= 0)
		{
			ChangeToTextColors();

			Console.SetCursorPosition(offset.Width, offset.Height);
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
	}

	public static void RenderLoadingScreen()
	{
		if (Settings.Current.ShowLoadingScreen &&
			Console.WindowHeight > Settings.Current.LoadingScreenLogo.Length + 5 &&
			Console.WindowWidth > Settings.Current.LoadingScreenLogo.Max(p => p.Length))
		{
			Console.Clear();

			var maxLength = Settings.Current.LoadingScreenLogo.Max(s => s.Length);

			for (int i = 0; i < Settings.Current.LoadingScreenLogo.Length; i++)
			{
				Console.SetCursorPosition((Console.WindowWidth - maxLength) / 2, (Console.WindowHeight - Settings.Current.LoadingScreenLogo.Length) / 2 + i);

				for (int j = 0; j < Settings.Current.LoadingScreenLogo[i].Length; j++)
				{
					var c = Settings.Current.LoadingScreenLogo[i][j];

					Console.ForegroundColor = c == '.' ?
						Settings.Current.DefaultBackgroundColor.Foreground :
						Settings.Current.DefaultContentColor.Foreground;
					Console.BackgroundColor = c == '.' ?
						Settings.Current.DefaultBackgroundColor.Background :
						Settings.Current.DefaultContentColor.Background;

					Console.Write(c);
				}
			}

			Console.ForegroundColor = Settings.Current.DefaultContentColor.Foreground;
			Console.BackgroundColor = Settings.Current.DefaultContentColor.Background;

			var splashText = Localizer.Localize(typeof(Renderer), null, Settings.Current.LoadingScreenSplashes[new Random().Next(Settings.Current.LoadingScreenSplashes.Length)]);

			Console.SetCursorPosition(Console.WindowWidth - (Console.WindowWidth - maxLength) / 2 - splashText.Length, (Console.WindowHeight - Settings.Current.LoadingScreenLogo.Length) / 2 - 2);

			Console.Write(splashText);

			var loadingText = Localizer.Localize(typeof(Renderer), null, "loading");

			Console.SetCursorPosition((Console.WindowWidth - loadingText.Length) / 2, (Console.WindowHeight + Settings.Current.LoadingScreenLogo.Length) / 2 + 2);

			Console.Write(loadingText);

			Task.Delay(Settings.Current.LoadingScreenDelay).Wait();
		}
	}

	private static Size ShrinkTableViewToConsoleSize(Table table, int verticalDecrement, int horizontalDecrement)
	{
		while (true)
		{
			var size = table.GetTableSize();

			if (Console.WindowWidth - horizontalDecrement < size.Width)
			{
				table.ViewOptions.DecreaseWidth();
			}
			else if (Console.WindowHeight - _FREE_LINES - verticalDecrement < size.Height)
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

		RenderCellBorders(placeHolderCell, placeHolderCellPosition, size, CellBorders.Get(CellBorderType.CornerCellClosed));

		RenderCellContent(placeHolderCell, placeHolderCellPosition, size, true, false);
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
		return Shared.IndexArray((int)Math.Ceiling((size.Width - 2) / (double)value.Length))
			.Aggregate("", (s, i) => s += value)[..(size.Width - 2)];
	}

	private static void RenderCornerCell(Table table, Size tableOffset)
	{
		var position = new Position(tableOffset.Width, tableOffset.Height);

		var size = table.GetCornerCellSize();

		RenderCellBorders(table.CornerCell, position, size, CellBorders.Get(CellBorderType.CornerCellOpen));

		RenderCellContent(table.CornerCell, position, size, true, false);
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

				RenderCellBorders(cell, position, size, border);

				RenderCellContent(cell, position, size, true, table.IsColumnSelected(x));
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

				RenderCellBorders(cell, position, size, border);

				RenderCellContent(cell, position, size, true, table.IsRowSelected(y));
			}
		});
	}

	private static void RenderContent(Table table, Size tableOffset, bool ignoreRenderingMode)
	{
		Shared.IndexArray(table.Size.Height).ForEach(y =>
			Shared.IndexArray(table.Size.Width).ForEach(x =>
			{
				var cell = table[x, y];

				if (cell.Visibility.IsVisible && table.IsCellInView(x, y))
				{
					var position = table.GetContentCellPosition(tableOffset, x, y);

					var size = table.GetContentCellSize(x, y);

					// var m = .3;

					// //IDEA
					// var ellipsisThreshold = (int)((Console.WindowWidth) * m);

					// size.Width = Math.Min(ellipsisThreshold, size.Width);

					// //IDEA limit size for content ellipses

					var posInView = table.PositionInView(x, y);

					var border = GetContentCellBorder(table, cell, posInView);

					RenderCellBorders(cell, position, size, border);

					RenderCellContent(cell, position, size, ignoreRenderingMode, false);
				}
			}));
	}

	private static CellBorder GetContentCellBorder(Table table, Cell cell, Position position)
	{
		var cellBorderType = GetContentCellBorderType(table.ViewOptions.Size, position, table.IsHeadLess);

		var border = CellBorders.Get(cellBorderType);

		return TrimContentCellBorder(border, table, cell, position);
	}

	private static CellBorder TrimContentCellBorder(CellBorder border, Table table, Cell cell, Position position)
	{
		var matrix = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 }, { 1, 1 } };

		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			if (GetNearestVisibleCell(table, position, matrix[i, 0], matrix[i, 1]) is var nearestCell && nearestCell is not null &&
				(nearestCell.Selection.IsPrimarySelected && cell.Selection.IsNotPrimarySelected || nearestCell.LayerIndex > cell.LayerIndex && cell.Selection.IsNotPrimarySelected))
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
		}

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

	private static CellBorderType GetContentCellBorderType(Size size, Position position, bool headlessTable)
	{
		if (position.X == size.Width - 1 && position.Y == size.Height - 1)
		{
			if (headlessTable && size.Height == 1)
			{
				return CellBorderType.ContentLeft;
			}

			return CellBorderType.ContentUpLeft;
		}
		else if (position.X == size.Width - 1)
		{
			if (headlessTable && position.Y == 0)
			{
				return CellBorderType.ContentDownLeft;
			}

			return CellBorderType.ContentVerticalLeft;
		}
		else if (position.Y == size.Height - 1)
		{
			if (headlessTable && position.X == 0)
			{
				return size.Height == 1 ? CellBorderType.ContentRight : CellBorderType.ContentUpRight;
			}

			return CellBorderType.ContentHorizontalUp;
		}
		else if (headlessTable && position.X == 0 && position.Y == 0)
		{
			return CellBorderType.ContentDownRight;
		}
		else if (headlessTable && position.X == 0)
		{
			return CellBorderType.ContentVerticalRight;
		}
		else if (headlessTable && position.Y == 0)
		{
			return CellBorderType.ContentHorizontalDown;
		}

		return CellBorderType.ContentOpen;
	}

	private static CellBorderType GetHeaderCellBorderType(int size, int position)
	{
		return position == size - 1 ? CellBorderType.HeaderVertical : CellBorderType.HeaderOpen;
	}

	private static CellBorderType GetSiderCellBorderType(int size, int position)
	{
		return position == size - 1 ? CellBorderType.SiderHorizontal : CellBorderType.SiderOpen;
	}

	private static void RenderCellBorders(Cell cell, Position position, Size size, CellBorder border)
	{
		ChangeToCellBorderColors(cell);

		if (border.Top != BorderType.None)
		{
			Console.SetCursorPosition(position.X + 1, position.Y);
			RenderBorderSegment(border.Top, size.Width - 2);
		}

		if (border.Left != BorderType.None)
		{
			Console.SetCursorPosition(position.X, position.Y + 1);
			Shared.IndexArray(size.Height - 2, 1).ForEach(i =>
			{
				RenderBorderSegment(border.Left);
				Shared.StepCursor(-1, 1);
			});
		}

		if (border.Right != BorderType.None)
		{
			Console.SetCursorPosition(position.X + size.Width - 1, position.Y + 1);
			Shared.IndexArray(size.Height - 2, 1).ForEach(i =>
			{
				RenderBorderSegment(border.Right);
				Shared.StepCursor(-1, 1);
			});
		}

		if (border.Bottom != BorderType.None)
		{
			Console.SetCursorPosition(position.X + 1, position.Y + size.Height - 1);
			RenderBorderSegment(border.Bottom, size.Width - 2);
		}

		if (border.TopLeft != BorderType.None)
		{
			Console.SetCursorPosition(position.X, position.Y);
			RenderBorderSegment(border.TopLeft);
		}

		if (border.TopRight != BorderType.None)
		{
			Console.SetCursorPosition(position.X + size.Width - 1, position.Y);
			RenderBorderSegment(border.TopRight);
		}

		if (border.BottomLeft != BorderType.None)
		{
			Console.SetCursorPosition(position.X, position.Y + size.Height - 1);
			RenderBorderSegment(border.BottomLeft);
		}

		if (border.BottomRight != BorderType.None)
		{
			Console.SetCursorPosition(position.X + size.Width - 1, position.Y + size.Height - 1);
			RenderBorderSegment(border.BottomRight);
		}
	}

	private static void RenderBorderSegment(BorderType border, int count = 1)
	{
		Console.Write(new string(BorderCharacters.Get(border), count));
	}

	private static void RenderCellContent(Cell cell, Position position, Size size, bool ignoreRenderingMode, bool showSelection)
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
						contentsToRender = [cell.LayerIndex.ToString()];
						hAlignment = HorizontalAlignment.Center;
						vAlignment = VerticalAlignment.Center;
					}
					break;

				case RenderingMode.Comment:
					{
						contentsToRender = cell.Comments;
						hAlignment = HorizontalAlignment.Center;
						vAlignment = VerticalAlignment.Center;
					}
					break;
			}
		}

		Shared.IndexArray(sizeWithoutBorders.Height).ForEach(i =>
		{
			Console.SetCursorPosition(position.X + 1, position.Y + 1 + i);


			// var m = .3;

			// //IDEA
			// var ellipsisThreshold = (int)((Console.WindowWidth) * m);

			// if (contentsToRender.Any(c => c.Length > (int)(Console.WindowWidth * m)))
			// {
			// 	sizeWithoutBorders.Width = (int)(Console.WindowWidth * m);
			// }

			// contentsToRender = contentsToRender.Select(c =>
			// {
			// 	return c.Length > ellipsisThreshold ? $"{c[..(ellipsisThreshold - 4)]} ..." : c;
			// });


			var content = "";
			var leftPaddingSize = sizeWithoutBorders.Width;
			var rightPaddingSize = 0;

			if (IsCellContentRenderNeeded(contentsToRender, vAlignment, cell.ContentPadding, i, sizeWithoutBorders.Height, out var contentIndex) && contentsToRender.Count() > contentIndex)
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

			ChangeToCellBackgroundColors(cell);
			ShowIndexCellSelection(showSelection);
			Console.Write(new string(cell.BackgroundCharacter, leftPaddingSize));

			ChangeToCellContentColors(cell);

			if (!ignoreRenderingMode)
			{
				switch (RendererSettings.RenderingMode)
				{
					case RenderingMode.Layer:
						{
							ChangeToLayerIndexContentColors(cell);
						}
						break;

					case RenderingMode.Comment:
						{
							ChangeToCommentContentColors(cell);
						}
						break;

					case RenderingMode.Content:
						{
							if (cell.ContentStyle.HasFlag(ContentStyle.Bold))
							{
								Console.Write(ContentStyleCharCodes.BOLD_CHAR_CODE);
							}
							if (cell.ContentStyle.HasFlag(ContentStyle.Dim))
							{
								Console.Write(ContentStyleCharCodes.DIM_CHAR_CODE);
							}
							if (cell.ContentStyle.HasFlag(ContentStyle.Italic))
							{
								Console.Write(ContentStyleCharCodes.ITALIC_CHAR_CODE);
							}
							if (cell.ContentStyle.HasFlag(ContentStyle.Underlined))
							{
								Console.Write(ContentStyleCharCodes.UNDERLINED_CHAR_CODE);
							}
							if (cell.ContentStyle.HasFlag(ContentStyle.Blinking))
							{
								Console.Write(ContentStyleCharCodes.BLINKING_CHAR_CODE);
							}
							if (cell.ContentStyle.HasFlag(ContentStyle.Striked))
							{
								Console.Write(ContentStyleCharCodes.STRIKED_CHAR_CODE);
							}
							if (cell.ContentStyle.HasFlag(ContentStyle.Overlined))
							{
								Console.Write(ContentStyleCharCodes.OVERLINED_CHAR_CODE);
							}
						}
						break;
				}
			}

			ShowIndexCellSelection(showSelection);
			Console.Write($"{content}{ContentStyleCharCodes.NORMAL_CHAR_CODE}");

			ChangeToCellBackgroundColors(cell);
			ShowIndexCellSelection(showSelection);
			Console.Write(new string(cell.BackgroundCharacter, rightPaddingSize));
		});
	}

	private static bool IsCellContentRenderNeeded(IEnumerable<object> contents, VerticalAlignment alignment, ContentPadding padding, int lineIndex, int height, out int contentIndex)
	{
		var startLineIndex = alignment switch
		{
			VerticalAlignment.Top => padding.Top,
			VerticalAlignment.Center => GetStartIndexForCenteredContent(height, contents.Count(), padding.Top, padding.Bottom),

			_ => height - contents.Count() - padding.Bottom
		};

		var isRenderNeeded = startLineIndex <= lineIndex && startLineIndex + contents.Count() > lineIndex;

		contentIndex = isRenderNeeded ? Math.Abs(startLineIndex - lineIndex) : default;

		return isRenderNeeded;
	}

	private static int GetStartIndexForCenteredContent(int contentAreaSize, int contentLength, int padding1, int padding2)
	{
		return Shared.Min(Shared.Max((contentAreaSize - contentLength) / 2, padding1), contentAreaSize - padding2 - contentLength);
	}

	public static void ShowIndexCellSelection(bool selected)
	{
		if (selected)
		{
			Console.ForegroundColor = Settings.Current.PrimarySelectionContentColor.Foreground;

			Console.BackgroundColor = Settings.Current.PrimarySelectionContentColor.Background;
		}
	}

	private static void ChangeToCommentContentColors(Cell cell)
	{
		Console.ForegroundColor = cell.Selection.IsPrimarySelected ?
			Settings.Current.PrimarySelectionContentColor.Foreground :
			Settings.Current.NotAvailableContentColor.Foreground;

		Console.BackgroundColor = cell.Selection.IsPrimarySelected ?
			Settings.Current.PrimarySelectionContentColor.Background :
			Settings.Current.NotAvailableContentColor.Background;
	}

	private static void ChangeToLayerIndexContentColors(Cell cell)
	{
		Console.ForegroundColor = cell.Selection.IsPrimarySelected ?
			Settings.Current.PrimarySelectionContentColor.Foreground :
				cell.LayerIndex == 0 ?
					Settings.Current.NotAvailableContentColor.Foreground :
						cell.ContentColor.Foreground;

		Console.BackgroundColor = cell.Selection.IsPrimarySelected ?
			Settings.Current.PrimarySelectionContentColor.Background :
				cell.LayerIndex == 0 ?
					Settings.Current.NotAvailableContentColor.Background :
						cell.ContentColor.Background;
	}

	private static void ChangeToCellContentColors(Cell cell)
	{
		var colors = cell.Selection.GetHighestSelectionLevel() switch
		{
			CellSelectionLevel.Primary => Settings.Current.PrimarySelectionContentColor,
			CellSelectionLevel.Secondary => Settings.Current.SecondarySelectionContentColor,
			CellSelectionLevel.Tertiary => Settings.Current.TertiarySelectionContentColor,

			_ => new(cell.ContentColor.Foreground, cell.ContentColor.Background)
		};

		Console.ForegroundColor = colors.Foreground;
		Console.BackgroundColor = colors.Background;
	}

	private static void ChangeToCellBackgroundColors(Cell cell)
	{
		var colors = cell.Selection.GetHighestSelectionLevel() switch
		{
			CellSelectionLevel.Primary => Settings.Current.PrimarySelectionBackgroundColor,
			CellSelectionLevel.Secondary => Settings.Current.SecondarySelectionBackgroundColor,
			CellSelectionLevel.Tertiary => Settings.Current.TertiarySelectionBackgroundColor,

			_ => new(cell.BackgroundColor.Foreground, cell.BackgroundColor.Background)
		};

		Console.ForegroundColor = colors.Foreground;
		Console.BackgroundColor = colors.Background;
	}

	private static void ChangeToCellBorderColors(Cell cell)
	{

		var colors = cell.Selection.GetHighestSelectionLevel() switch
		{
			CellSelectionLevel.Primary => Settings.Current.PrimarySelectionBorderColor,
			CellSelectionLevel.Secondary => Settings.Current.SecondarySelectionBorderColor,
			CellSelectionLevel.Tertiary => Settings.Current.TertiarySelectionBorderColor,

			_ => new(cell.BorderColor.Foreground, cell.BorderColor.Background)
		};

		Console.ForegroundColor = colors.Foreground;
		Console.BackgroundColor = colors.Background;
	}

	public static void ChangeToTextColors()
	{
		Console.ForegroundColor = Settings.Current.TextColor.Foreground;
		Console.BackgroundColor = Settings.Current.TextColor.Background;
	}

	public static void ChangeToOkLabelColors()
	{
		Console.ForegroundColor = Settings.Current.OkLabelColor.Foreground;
		Console.BackgroundColor = Settings.Current.OkLabelColor.Background;
	}

	public static void ChangeToNotOkLabelColors()
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