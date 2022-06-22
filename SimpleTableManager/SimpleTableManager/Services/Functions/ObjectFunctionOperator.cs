using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Services.Functions;

[JsonConverter(typeof(StringEnumConverter))]
public enum ObjectFunctionOperator
{
	Const
}