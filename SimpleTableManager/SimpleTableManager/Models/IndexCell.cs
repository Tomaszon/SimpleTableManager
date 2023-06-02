﻿using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class IndexCell : Cell
	{
		public char LowerArrow { get; set; }

		public char HigherArrow { get; set; }

		public int Index { get; set; }

		public IndexCell(Table table, int index, char lowerArrow, char higherArrow) : base(table, index.ToString())
		{
			Index = index;
			LowerArrow = lowerArrow;
			HigherArrow = higherArrow;
		}

		public void AppendHigherEllipsis(int width)
		{
			SetContent($"{GetContents().ElementAt(0)} {HigherArrow} {width - 1}");
		}

		public void AppendLowerEllipsis()
		{
			SetContent($"0 {LowerArrow} {GetContents().ElementAt(0)}");
		}

		public void RemoveEllipses()
		{
			SetContent(Index.ToString());
		}

		public void ShowSelection(bool selected)
		{
			ContentColor.Foreground = selected ?
				Settings.Current.SelectedContentColor.Foreground : Settings.Current.DefaultContentColor.Foreground;
			ContentColor.Background = selected ?
				Settings.Current.SelectedContentColor.Background : Settings.Current.DefaultContentColor.Background;
		}
	}
}