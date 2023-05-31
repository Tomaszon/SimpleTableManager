using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum NumericFunctionOperator
{
	Const,
	Neg,
	Abs,
	Sum,
	Sub,
	Avg,
	Min,
	Max,
	Floor,
	Ceiling,
	Round,
	Mul,
	Div,
	Rem,
	And,
	Or
}