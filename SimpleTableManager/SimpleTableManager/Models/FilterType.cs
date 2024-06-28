namespace SimpleTableManager.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum FilterType
{
	Column,
	Row
}