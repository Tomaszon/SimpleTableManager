namespace SimpleTableManager.Models.CommandExecuters
{
	public partial class Cell
	{
		[CommandFunction, CommandShortcut("resetCellContent")]
		public void ResetContent()
		{
			Deselect();
			
			UpdateReferenceSubscription(ContentFunction, null);

			ContentFunction = null;

			Select();
		}

		[CommandFunction]
		public void ResetComment()
		{
			Comments.Clear();
		}

		[CommandFunction, CommandShortcut("resetCellFormat")]
		public void ResetFormat()
		{
			GivenSize = new Size(7, 1);
			ContentPadding = new();
			ContentAlignment = new(HorizontalAlignment.Center, VerticalAlignment.Center);
			ContentColor = new(Settings.Current.DefaultContentColor);
			BorderColor = new(Settings.Current.DefaultBorderColor);
			BackgroundColor = new(Settings.Current.DefaultBackgroundColor);
			BackgroundCharacter = Settings.Current.DefaultCellBackgroundCharacter;
			LayerIndex = 0;
			ContentStyle = ContentStyle.Normal;
		}

		[CommandFunction(IgnoreReferencedObject = true)]
		public void ResetReferenceCell()
		{
			ReferencedObject = null;
		}
	}
}