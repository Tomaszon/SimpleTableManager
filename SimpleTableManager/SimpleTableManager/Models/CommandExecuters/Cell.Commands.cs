using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction, CommandShortcut("copyCellContent")]
	public void CopyContent()
	{
		//IDEA use cell guid instead of cell position
		Table.Document.GlobalStorage.Add("cellContent", (Table[this], ContentFunction));
	}

	[CommandFunction, CommandShortcut("cutCellContent")]
	public void CutContent()
	{
		//IDEA use cell guid instead of cell position
		Table.Document.GlobalStorage.Add("cellContent", (Table[this], ContentFunction));

		ResetContent();
	}

	[CommandFunction, CommandShortcut("pasteCellContent")]
	public void PasteContent()
	{
		var stored = Table.Document.GlobalStorage.TryGet<(Position, IFunction)>("cellContent");

		var diff = Table[this] - stored.Item1;

		stored.Item2.ShiftferenceArgumentPositions(diff);

		SetContent(stored.Item2);
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