namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum Shape3dOperator
{
	Const,
	Area,
	Volume
}