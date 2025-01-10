namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum Shape3dOperator
{
	Const,
	Area,
	Volume
}