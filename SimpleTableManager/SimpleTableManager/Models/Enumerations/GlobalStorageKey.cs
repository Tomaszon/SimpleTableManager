namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum GlobalStorageKey
{
	None,
	CellContent,
	CellFormat
}