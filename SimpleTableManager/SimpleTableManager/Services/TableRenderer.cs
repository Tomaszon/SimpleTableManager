using System.Collections.Generic;
using System.Text;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class TableRenderer
	{
		public static string Render(Table table, ViewOptions viewOptions = null)
		{
			StringBuilder sb = new StringBuilder();

			Size cellSize = new Size(7, 1);

			viewOptions ??= new ViewOptions(0, 0, table.Size.Width, table.Size.Height);

			DrawHeaderRow(sb, viewOptions, cellSize);

			for (int y = viewOptions.Position.Y; y < viewOptions.Position.Y + viewOptions.Size.Height - 1; y++)
			{
				var cellsInRow = table.Cells.GetRange(y * table.Size.Width, table.Size.Width);

				DrawInnerRows(sb, y, cellSize, cellsInRow.GetRange(viewOptions.Position.X, viewOptions.Size.Width));
			}

			var cellsInLastRow = table.Cells.GetRange(table.Cells.Count - table.Size.Width, table.Size.Width);

			DrawLastRow(sb, table.Size.Height - 1, cellSize, cellsInLastRow.GetRange(viewOptions.Position.X, viewOptions.Size.Width));

			return sb.ToString();
		}

		private static void DrawHeaderRow(StringBuilder sb, ViewOptions viewOptions, Size cellSize)
		{
			sb.Append(TableBorderCharacter.DoubleRight_DoubleDown);
			sb.Append(new string(TableBorderCharacter.DoubleLeft_DoubleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleLeft_DoubleRight_DoubleDown);
			for (int i = 0; i < viewOptions.Size.Width - 1; i++)
			{
				sb.Append(new string(TableBorderCharacter.DoubleLeft_DoubleRight, cellSize.Width));
				sb.Append(TableBorderCharacter.DoubleLeft_DoubleRight_SingleDown);
			}
			sb.Append(new string(TableBorderCharacter.DoubleLeft_DoubleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleLeft_SingleDown);
			sb.AppendLine();

			sb.Append(TableBorderCharacter.DoubleUp_DoubleDown);
			sb.Append(@"y \ x".PadLeftRight(cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleUp_DoubleDown);
			for (int i = viewOptions.Position.X; i < viewOptions.Position.X + viewOptions.Size.Width; i++)
			{
				sb.Append($"{i + 1}".PadLeftRight(cellSize.Width));
				sb.Append(TableBorderCharacter.SingleUp_SingleDown);
			}
			sb.AppendLine();

			sb.Append(TableBorderCharacter.DoubleUp_DoubleRight_DoubleDown);
			sb.Append(new string(TableBorderCharacter.DoubleLeft_DoubleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleUp_DoubleLeft_DoubleRight_DoubleDown);
			for (int i = 0; i < viewOptions.Size.Width - 1; i++)
			{
				sb.Append(new string(TableBorderCharacter.DoubleLeft_DoubleRight, cellSize.Width));
				sb.Append(TableBorderCharacter.SingleUp_DoubleLeft_DoubleRight_SingleDown);
			}
			sb.Append(new string(TableBorderCharacter.DoubleLeft_DoubleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.SingleUp_DoubleLeft_SingleDown);
			sb.AppendLine();
		}

		private static void DrawInnerRows(StringBuilder sb, int row, Size cellSize, List<Cell> cells)
		{
			sb.Append(TableBorderCharacter.DoubleUp_DoubleDown);
			sb.Append($"{row + 1}".PadLeftRight(cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleUp_DoubleDown);
			foreach (var cell in cells)
			{
				sb.Append(cell.Content.ToString().PadLeftRight(cell.Size.Width));
				sb.Append(TableBorderCharacter.SingleUp_SingleDown);
			}
			sb.AppendLine();

			sb.Append(TableBorderCharacter.DoubleUp_SingleRight_DoubleDown);
			sb.Append(new string(TableBorderCharacter.SingleLeft_SingleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleUp_SingleLeft_SingleRight_DoubleDown);
			for (int i = 0; i < cells.Count - 1; i++)
			{
				sb.Append(new string(TableBorderCharacter.SingleLeft_SingleRight, cellSize.Width));
				sb.Append(TableBorderCharacter.SingleUp_SingleLeft_SingleRight_SingleDown);
			}
			sb.Append(new string(TableBorderCharacter.SingleLeft_SingleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.SingleUp_SingleLeft_SingleDown);
			sb.AppendLine();
		}

		private static void DrawLastRow(StringBuilder sb, int row, Size cellSize, List<Cell> cells)
		{
			sb.Append(TableBorderCharacter.DoubleUp_DoubleDown);
			sb.Append($"{row + 1}".PadLeftRight(cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleUp_DoubleDown);
			foreach (var cell in cells)
			{
				sb.Append(cell.Content.ToString().PadLeftRight(cell.Size.Width));
				sb.Append(TableBorderCharacter.SingleUp_SingleDown);
			}
			sb.AppendLine();

			sb.Append(TableBorderCharacter.DoubleUp_SingleRight);
			sb.Append(new string(TableBorderCharacter.SingleLeft_SingleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.DoubleUp_SingleLeft_SingleRight);
			for (int i = 0; i < cells.Count - 1; i++)
			{
				sb.Append(new string(TableBorderCharacter.SingleLeft_SingleRight, cellSize.Width));
				sb.Append(TableBorderCharacter.SingleUp_SingleLeft_SingleRight);
			}
			sb.Append(new string(TableBorderCharacter.SingleLeft_SingleRight, cellSize.Width));
			sb.Append(TableBorderCharacter.SingleUp_SingleLeft);
		}
	}
}
