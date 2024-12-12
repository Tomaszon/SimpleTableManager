using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction, CommandShortcut("copyCellContent")]
	public void CopyContent()
	{
		Table.Document.GlobalStorage.Add("cellContent", ContentFunction);
	}

	[CommandFunction, CommandShortcut("cutCellContent")]
	public void CutContent()
	{
		Table.Document.GlobalStorage.Add("cellContent", ContentFunction);

		ResetContent();
	}

	[CommandFunction, CommandShortcut("pasteCellContent")]
	public void PasteContent()
	{
		var stored = Table.Document.GlobalStorage.TryGet<IFunction>("cellContent");

		if (stored is not null)
		{
			ContentFunction = stored;
		}
	}

	[CommandFunction, CommandShortcut("copyCellFormat")]
	public void CopyFormat()
	{
		Table.Document.GlobalStorage.Add("cellFormat",
		(
			GivenSize,
			ContentPadding,
			ContentAlignment,
			ContentColor,
			BorderColor,
			LayerIndex
		));
	}

	[CommandFunction, CommandShortcut("pasteCellFormat")]
	public void PasteFormat()
	{
		var stored = Table.Document.GlobalStorage.TryGet<ValueTuple<Size, ContentPadding, ContentAlignment, ConsoleColorSet, ConsoleColorSet, int>?>("cellFormat");

		if (stored is not null)
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