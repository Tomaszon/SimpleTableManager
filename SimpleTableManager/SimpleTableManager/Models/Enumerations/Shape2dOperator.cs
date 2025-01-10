namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum Shape2dOperator
{
	Const,
	Area,
	Perimeter
}
