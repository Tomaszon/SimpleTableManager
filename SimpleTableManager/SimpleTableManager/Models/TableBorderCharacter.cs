using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TableBorderCharacter
	{
		SingleUp_SingleDown,
		SingleUp_SingleLeft,
		SingleLeft_SingleRight,
		DoubleLeft_SingleDown,
		DoubleRight_DoubleDown,
		DoubleLeft_DoubleRight,
		DoubleLeft_DoubleRight_DoubleDown,
		DoubleLeft_DoubleRight_SingleDown,
		DoubleUp_DoubleDown,
		DoubleUp_DoubleRight_DoubleDown,
		DoubleUp_DoubleLeft_DoubleRight_DoubleDown,
		SingleUp_DoubleLeft_DoubleRight_SingleDown,
		SingleUp_DoubleLeft_SingleDown,
		DoubleUp_SingleRight_DoubleDown,
		DoubleUp_SingleLeft_SingleRight_DoubleDown,
		SingleUp_SingleLeft_SingleRight_SingleDown,
		SingleUp_SingleLeft_SingleDown,
		DoubleUp_SingleRight,
		DoubleUp_SingleLeft_SingleRight,
		SingleUp_SingleLeft_SingleRight
	}
}
