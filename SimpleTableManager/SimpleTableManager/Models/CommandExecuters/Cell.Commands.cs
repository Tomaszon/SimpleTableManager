using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction(StateModifier = false), CommandShortcut("copyCellContent")]
	public void CopyContent()
	{
		//IDEA use cell guid instead of cell position
		Table.Document.GlobalStorage.Add(GlobalStorageKey.CellContent, (Table[this], ContentFunction));
	}

	[CommandFunction(StateModifier = false)]
	public void CopyContentTo(Position position, string? tableName = null)
	{
		var table = tableName is not null ? Table.Document[tableName] : Table;

		ThrowIf(table is null, $"No table found with name {tableName}");

		var clone = Shared.SerializeClone(ContentFunction);

		var diff = position - Table[this];

		var targetCell = table[position];

		targetCell.SetContent(clone, diff, false);
		targetCell.InvokeStateModifierCommandExecutedEvent(new(targetCell));
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

	[CommandFunction]
	public void PasteContentFrom(Position position, string? tableName = null)
	{
		var table = tableName is not null ? Table.Document[tableName] : Table;

		ThrowIf(table is null, $"No table found with name {tableName}");

		var sourceCell = table[position];

		var clone = Shared.SerializeClone(sourceCell.ContentFunction);

		var diff = Table[this] - position;

		SetContent(clone, diff);
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