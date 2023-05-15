using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Services.Functions;

[JsonConverter(typeof(StringEnumConverter))]
public enum NumericFunctionOperator
{
	Const,
	Sum,
	Avg,
	Min,
	Max,
	Floor,
	Ceiling,
	Round
}