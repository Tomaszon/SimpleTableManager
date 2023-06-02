namespace SimpleTableManager.Models.Enumerations
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RenderingMode
	{
		Content,
		Layer
	}
}