namespace SimpleTableManager.Models.Enumerations;

[Flags]
public enum ContentStyle
{
	Normal = 0,
	Bold = 1,
	Italic = 2,
	Underlined = 4,
	Blinking = 8,
	All = Blinking | Underlined | Italic | Bold
}