namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum ArgumentFilter
{
	All,
	Named,
	Unnamed
}