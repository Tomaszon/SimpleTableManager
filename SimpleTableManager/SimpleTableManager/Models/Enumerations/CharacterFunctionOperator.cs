namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum CharacterFunctionOperator
{
	Const,
	Concat,
	Join,
	Repeat
}