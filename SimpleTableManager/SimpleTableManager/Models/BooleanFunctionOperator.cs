using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum BooleanFunctionOperator
{
	Const,
	Not,
	And,
	Or
}