using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Models.Enumerations
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum HorizontalAlignment
	{
		Left,
		Center,
		Right
	}
}
