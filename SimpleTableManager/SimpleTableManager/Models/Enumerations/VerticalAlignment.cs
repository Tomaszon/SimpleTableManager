namespace SimpleTableManager.Models.Enumerations
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum VerticalAlignment
	{
		Top,
		Center,
		Bottom
	}
}
