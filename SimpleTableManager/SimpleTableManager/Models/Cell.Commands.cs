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

	[CommandReference, CommandShortcut("cutCellContent")]
	public void CutContent()
	{
		GlobalContainer.Add("cellContent", ContentFunction);

		ResetContent();
	}

	[CommandReference, CommandShortcut("pasteCellContent")]
	public void PasteContent()
	{
		var stored = GlobalContainer.TryGet<IFunction>("cellContent");

		if (stored is not null)
		{
			ContentFunction = stored;
		}
	}

	[CommandReference, CommandShortcut("copyCellFormat")]
	public void CopyFormat()
	{
		GlobalContainer.Add("cellFormat",
		(
			GivenSize,
			ContentPadding,
			ContentAlignment,
			ContentColor,
			BorderColor,
			LayerIndex
		));
	}

	[CommandReference, CommandShortcut("pasteCellFormat")]
	public void PasteFormat()
	{
		var stored = GlobalContainer.TryGet<ValueTuple<Size, ContentPadding, ContentAlignment, ConsoleColorSet, ConsoleColorSet, int>?>("cellFormat");

		if(stored is not null)
		{
			GivenSize = stored.Value.Item1;
			ContentPadding = stored.Value.Item2;
			ContentAlignment = stored.Value.Item3;
			ContentColor = stored.Value.Item4;
			BorderColor = stored.Value.Item5;
			LayerIndex = stored.Value.Item6;
		}
	}
}