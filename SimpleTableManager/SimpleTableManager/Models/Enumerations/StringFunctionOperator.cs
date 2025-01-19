namespace SimpleTableManager.Models.Enumerations;

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
	Like
}