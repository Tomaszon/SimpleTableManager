namespace SimpleTableManager.Models.Enumerations
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ArgumentName
	{
		Type,
		Divider,
		Separator,
		Decimals,
		Trim,
		Count,
		Format
	}
}