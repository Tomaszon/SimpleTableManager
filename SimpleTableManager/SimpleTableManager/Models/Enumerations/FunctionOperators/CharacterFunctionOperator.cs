namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum CharacterFunctionOperator
{
	Const,
	Concat,
	Join,
	Repeat,
	Min,
	Max,
	Greater,
	Less,
	GreaterOrEquals,
	LessOrEquals,
	Equals,
	NotEquals
}