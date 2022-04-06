using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public void SetContent(params object[] contents)
		{
			Content = contents.ToList();
		}

		[CommandReference]
		public void SetVerticalAlignment(VertialAlignment alignment)
		{
			VertialAlignment = alignment;
		}

		[CommandReference]
		public void SetHorizontalAlignment(HorizontalAlignment alignment)
		{
			HorizontalAlignment = alignment;
		}

		[CommandReference]
		public void SetPadding(int top, int bottom, int left, int right)
		{
			Padding = new ContentPadding(top, bottom, left, right);
		}

		[CommandReference]
		public void SetVerticalPadding(int top, int bottom)
		{
			Padding = new ContentPadding(top, bottom, Padding.Left, Padding.Right);
		}

		[CommandReference]
		public void SetHorizontalPadding(int left, int right)
		{
			Padding = new ContentPadding(Padding.Top, Padding.Bottom, left, right);
		}
	}
}
