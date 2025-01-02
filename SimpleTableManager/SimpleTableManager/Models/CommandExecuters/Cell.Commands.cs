using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction, CommandShortcut("copyCellContent")]
	public void CopyContent()
	{
		//IDEA use cell guid instead of cell position
		Table.Document.GlobalStorage.Add(GlobalStorageKey.CellContent, (Table[this], ContentFunction));
	}

	[CommandFunction, CommandShortcut("cutCellContent")]
	public void CutContent()
	{
		//IDEA use cell guid instead of cell position
		Table.Document.GlobalStorage.Add(GlobalStorageKey.CellContent, (Table[this], ContentFunction));

		ResetContent();
	}

	[CommandFunction, CommandShortcut("pasteCellContent")]
	public void PasteContent()
	{
		var stored = Table.Document.GlobalStorage.TryGet<(Position, IFunction)>(GlobalStorageKey.CellContent);

		var diff = stored.Item1 is not null ? Table[this] - stored.Item1 : null;

		SetContent(stored.Item2, diff);
	}

	[CommandFunction, CommandShortcut("copyCellFormat")]
	public void CopyFormat()
	{
		Table.Document.GlobalStorage.Add(GlobalStorageKey.CellFormat,
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
		var stored = Table.Document.GlobalStorage.TryGet<ValueTuple<Size, ContentPadding, ContentAlignment, ConsoleColorSet, ConsoleColorSet, int>?>(GlobalStorageKey.CellFormat);

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