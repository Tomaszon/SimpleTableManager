namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction(StateModifier = false), CommandShortcut("copyCellContent")]
	public void CopyContent()
	{
		Table.Document.GlobalStorage.Add(GlobalStorageKey.CellContent, (Table[this], ContentFunction));
	}

	[CommandFunction(StateModifier = false)]
	public void CopyContentTo([MinLength(1)] IEnumerable<Position> positions)
	{
		CopyContentTo(Table, positions);
	}

	[CommandFunction("copyContentToTable", StateModifier = false)]
	public void CopyContentTo(string tableName, [MinLength(1)] IEnumerable<Position> positions)
	{
		var table = Table.Document[tableName];

		ThrowIf(table is null, $"No table found with name {tableName}");

		CopyContentTo(table, positions);
	}

	[CommandFunction(StateModifier = false)]
	public void CopyContentToRange(Position positionFrom, Position positionTo, string? tableName = null)
	{
		var table = tableName is not null ? Table.Document[tableName] : Table;

		ThrowIf(table is null, $"No table found with name {tableName}");

		CopyContentTo(table, table[positionFrom, positionTo].Select(c => table[c]));
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
			BackgroundColor,
			BackgroundCharacter,
			LayerIndex,
			ContentStyle
		));
	}

	private void CopyContentTo(Table table, [MinLength(1)] IEnumerable<Position> positions)
	{
		positions.ForEach(p =>
		{
			var clone = Shared.SerializeClone(ContentFunction);

			var diff = p - Table[this];

			var targetCell = table[p];

			targetCell.SetContent(clone, diff, false);
			targetCell.InvokeStateModifierCommandExecutedEvent(new(targetCell));
		});
	}
}