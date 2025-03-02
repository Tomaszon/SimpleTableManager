namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum ChartFunctionOperator
{
	Raw,
	Scatter,
	Column,
	Bar
}