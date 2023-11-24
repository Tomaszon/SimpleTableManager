namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum StringFunctionOperator
{
	Const,
	Con,
	Join,
	Len,
	Split,
	Trim,
	Blow
}