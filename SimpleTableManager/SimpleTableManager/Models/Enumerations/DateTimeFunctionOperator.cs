namespace SimpleTableManager.Models.Enumerations;

[JsonConverter(typeof(StringEnumConverter))]
public enum DateTimeFunctionOperator
{
	Const,
	Offset,
	Difference,
	Sum,
	Sub,
	Now,
	Years,
	Months,
	Days,
	Hours,
	Minutes,
	Seconds,
	Milliseconds,
	TotalYears,
	TotalMonths,
	TotalDays,
	TotalHours,
	TotalMinutes,
	TotalSeconds,
	TotalMilliseconds
}