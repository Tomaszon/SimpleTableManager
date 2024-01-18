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
	}
}