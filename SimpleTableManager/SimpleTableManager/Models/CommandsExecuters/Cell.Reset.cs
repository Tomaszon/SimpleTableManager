using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters
{
	public partial class Cell
	{
		[CommandFunction, CommandShortcut("resetCellContent")]
		public void ResetContent()
		{
			ContentFunction = null;
		}

		[CommandFunction]
		public void ResetComment()
		{
			Comment = null;
		}

		[CommandFunction, CommandShortcut("resetCellFormat")]
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