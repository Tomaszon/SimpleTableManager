namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum BooleanFunctionOperator
{
	Const,
	Not,
	And,
	Or,
	IsNull,
	IsNotNull,
	Greater,
	Less,
	GreaterOrEquals,
	LessOrEquals,
	Equals,
	NotEquals
}