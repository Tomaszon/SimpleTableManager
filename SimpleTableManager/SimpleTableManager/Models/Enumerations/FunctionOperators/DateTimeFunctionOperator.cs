namespace SimpleTableManager.Models.Enumerations.FunctionOperators;

[JsonConverter(typeof(StringEnumConverter))]
public enum DateTimeFunctionOperator
{
	Const,
	Offset,
	Avg,
	Sum,
	Sub,
	Mul,
	Div,
	Now,
	Years,
	Months,
	Days,
	Hours,
	Minutes,
	Seconds,
	Milliseconds,
	TotalDays,
	TotalHours,
	TotalMinutes,
	TotalSeconds,
	TotalMilliseconds,
	Greater,
	Less,
	GreaterOrEquals,
	LessOrEquals,
	Equals,
	NotEquals
}