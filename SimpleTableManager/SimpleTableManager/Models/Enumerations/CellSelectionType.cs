namespace SimpleTableManager.Models.Enumerations;

[Flags]
[JsonConverter(typeof(StringEnumConverter))]
public enum CellSelectionType
{
	None = 0,
	Primary = 1,
	Secondary = 2,
	Tertiary = 4
}