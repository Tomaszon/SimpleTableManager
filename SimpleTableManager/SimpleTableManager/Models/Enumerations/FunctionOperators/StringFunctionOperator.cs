namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum StringFunctionOperator
{
	Const,
	Concat,
	Join,
	Len,
	Split,
	Trim,
	Blow,
	Like,
	Greater,
	Less,
	GreaterOrEquals,
	LessOrEquals,
	Equals,
	NotEquals
}