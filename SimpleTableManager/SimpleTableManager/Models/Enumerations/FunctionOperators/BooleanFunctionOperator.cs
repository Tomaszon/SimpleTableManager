namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum BooleanFunctionOperator
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