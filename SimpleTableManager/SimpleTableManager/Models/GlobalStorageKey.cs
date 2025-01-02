namespace SimpleTableManager.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum GlobalStorageKey
{
	None,
	CellContent,
	CellFormat
}