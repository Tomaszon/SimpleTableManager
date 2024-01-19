using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models;

public partial class Cell
{
	[CommandReference, CommandShortcut("copyCellContent")]
	public void CopyContent()
	{
		GlobalContainer.Add("cellContent", ContentFunction);
	}

	[CommandReference, CommandShortcut("pasteCellContent")]
	public void PasteContent()
	{
		ContentFunction = GlobalContainer.TryGet<IFunction>("cellContent");
	}
}