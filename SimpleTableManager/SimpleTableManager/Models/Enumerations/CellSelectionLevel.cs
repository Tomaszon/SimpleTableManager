namespace SimpleTableManager.Models.Enumerations;

[Flags]
[JsonConverter(typeof(StringEnumConverter))]
public enum CellSelectionLevel
{
	None = 0,
	Primary = 1,
	Secondary = 2,
	Tertiary = 4,
	NotPrimary = Secondary | Tertiary,
	NotSecondary = Primary | Tertiary,
	NotTertiary = Primary | Secondary,
	All = Primary | Secondary | Tertiary
}