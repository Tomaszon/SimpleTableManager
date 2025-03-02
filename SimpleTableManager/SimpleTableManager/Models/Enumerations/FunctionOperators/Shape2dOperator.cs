namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum Shape2dOperator
{
	Const,
	Area,
	Perimeter
}
