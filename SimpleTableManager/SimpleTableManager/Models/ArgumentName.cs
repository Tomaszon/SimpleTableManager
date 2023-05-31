using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ArgumentName
	{
		Type,
		Divider,
		Separator,
		Decimals
	}
}