namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum LogicalFunctionOperator
{
	Const,
	Not,
	And,
	Or,
	IsNull,
	IsNotNull,
	Min,
	Max,
	Greater,
	Less,
	GreaterOrEquals,
	LessOrEquals,
	Equals,
	NotEquals
}