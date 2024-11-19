namespace SimpleTableManager.Models.Enumerations;

[Flags]
public enum ContentStyle
{
	Normal = 0,
	Bold = 1,
	Dim = 2,
	Italic = 4,
	Underlined = 8,
	Blinking = 16,
	Striked = 32,
	Overlined = 64,
	All = Overlined | Striked | Blinking | Underlined | Italic | Dim | Bold
}