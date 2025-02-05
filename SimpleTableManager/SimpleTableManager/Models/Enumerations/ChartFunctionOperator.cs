namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum ChartFunctionOperator
{
	Raw,
	Scatter,
	Column,
	Bar
}