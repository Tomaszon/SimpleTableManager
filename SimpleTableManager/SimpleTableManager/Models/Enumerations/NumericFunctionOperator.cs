namespace SimpleTableManager.Models.Enumerations;

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
	Or,
	Pow,
	Sqrt,
	Log2,
	Log10,
	LogE,
	LogN
}