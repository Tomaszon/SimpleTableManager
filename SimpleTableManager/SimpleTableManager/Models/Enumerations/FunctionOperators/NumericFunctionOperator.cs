namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

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
	Round,
	Floor,
	Ceiling,
	Rem,
	And,
	Or,
	Mul,
	Div,
	Pow,
	Sqrt,
	Log2,
	Log10,
	LogE,
	LogN,
	Greater,
	Less,
	Equals,
	NotEquals,
	GreaterOrEquals,
	LessOrEquals
}