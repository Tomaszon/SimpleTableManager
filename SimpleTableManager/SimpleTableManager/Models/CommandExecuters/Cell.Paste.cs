using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
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