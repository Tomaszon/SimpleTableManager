using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Services.Functions;

[JsonConverter(typeof(StringEnumConverter))]
public enum BooleanFunctionOperator
{
	Not,
	And,
	Or
}