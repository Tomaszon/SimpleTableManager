using System.Collections.Generic;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public void SetContent(params object[] contents)
		{
			Content = new List<object>(contents);
		}

		[CommandReference]
		public void SetVerticalAlignment(VertialAlignment alignment)
		{
			VertialAlignment = alignment;
		}

		[CommandReference]
		public void SetHorizontalAlignment(HorizontalAlignment alignment)
		{
			HorizontalAlignment= alignment;
		}
	}
}
