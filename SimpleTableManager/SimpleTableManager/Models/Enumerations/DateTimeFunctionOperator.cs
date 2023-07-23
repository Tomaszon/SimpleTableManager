namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum DateTimeFunctionOperator
{
	Const,
	Sum,
	Now
}