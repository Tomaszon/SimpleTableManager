namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum StringFunctionOperator
{
	Const,
	Concat,
	Join,
	Split,
	Trim,
	Len,
	Blow,
	Like,
	Min,
	Max,
	Greater,
	Less,
	GreaterOrEquals,
	LessOrEquals,
	Equals,
	NotEquals
}