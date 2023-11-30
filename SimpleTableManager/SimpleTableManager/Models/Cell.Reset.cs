namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public void ResetContent()
		{
			ContentFunction = null;
		}
	}
}