namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum ArgumentName
{
	Divider,
	Separator,
	Decimals,
	Trim,
	Count,
	Power,
	Base,
	Format,
	Pattern,
	First,
	Last,
	Offset,
	Reference
}