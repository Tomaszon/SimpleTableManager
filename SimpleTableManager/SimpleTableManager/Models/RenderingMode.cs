using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RenderingMode
	{
		Content,
		LayerIndex
	}
}