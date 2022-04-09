﻿using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class IndexCell : Cell
	{
		public char LowerArrow { get; set; }

		public char HigherArrow { get; set; }

		public int Index { get; set; }

		public IndexCell(int index, char lowerArrow, char higherArrow) : base(index)
		{
			Index = index;
			LowerArrow = lowerArrow;
			HigherArrow = higherArrow;
		}

		public void AppendHigherEllipsis(int width)
		{
			SetContent($"{Content[0]} {HigherArrow} {width - 1}");
		}

		public void AppendLowerEllipsis()
		{
			SetContent($"0 {LowerArrow} {Content[0]}");
		}

		public void Normalize()
		{
			SetContent(Index);
		}

		public void ShowSelection(bool selected)
		{
			ContentColor.Foreground = selected ? 
				Settings.Current.SelectedContentColor.Foreground : Settings.Current.DefaultContentColor.Foreground;
		}
	}
}