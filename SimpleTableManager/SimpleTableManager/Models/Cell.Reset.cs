using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference, CommandShortcut("resetCellContent")]
		public void ResetContent()
		{
			ContentFunction = null;
		}

		[CommandReference]
		public void ResetComment()
		{
			Comment = null;
		}

		[CommandReference, CommandShortcut("resetCellFormat")]
		public void ResetFormat()
		{
			GivenSize = new Size(7, 1);
			ContentPadding = new();
			ContentAlignment = (HorizontalAlignment.Center, VerticalAlignment.Center);
			ContentColor = new(Settings.Current.DefaultContentColor);
			BorderColor = new(Settings.Current.DefaultBorderColor);
			LayerIndex = 0;
		}
	}
}