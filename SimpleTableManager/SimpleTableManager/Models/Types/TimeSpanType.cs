namespace SimpleTableManager.Models.Types;

[ParseFormat("d", "^(?<d>\\d+)$")]
[ParseFormat("d.hh", "^(?<d>\\d+)\\.(?<h>\\d{2})$")]
[ParseFormat("d.hh:mm:ss.fff", "^((?<d>\\d+)\\.)?(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class TimeSpanType(TimeSpan timeSpan) :
	TypeBase<TimeSpanType, TimeSpan>(timeSpan),
	IParsable<TimeSpanType>,
	IParsableCore<TimeSpanType>
{
	public IntegerType Days => Value.Days;

	public FractionType TotalDays => Value.TotalDays;

	public IntegerType Hours => Value.Hours;

	public FractionType TotalHours => Value.TotalHours;

	public IntegerType Minutes => Value.Minutes;

	public FractionType TotalMinutes => Value.TotalMinutes;

	public IntegerType Seconds => Value.Seconds;

	public FractionType TotalSeconds => Value.TotalSeconds;

	public IntegerType Milliseconds => Value.Milliseconds;

	public FractionType TotalMilliseconds => Value.TotalMilliseconds;

	public TimeSpanType(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) : this(new(days, hours, minutes, seconds, milliseconds)) { }

	public static TimeSpanType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return TimeSpan.Parse(args["0"].Value, formatProvider);
	}

	public static implicit operator TimeSpan(TimeSpanType timeSpan)
	{
		return timeSpan.Value;
	}

	public static implicit operator TimeSpanType(TimeSpan timeSpan)
	{
		return new(timeSpan);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return format switch
		{
			"hh:mm" => $"{Hours:00}:{Minutes:00}",
			"hh:mm:ss" => $"{Hours:00}:{Minutes:00}:{Seconds:00}",
			"hh:mm:ss.f" => $"{Hours:00}:{Minutes:00}:{Seconds:00}.{Milliseconds:0}",
			"hh:mm:ss.ff" => $"{Hours:00}:{Minutes:00}:{Seconds:00}.{Milliseconds:00}",
			"hh:mm:ss.fff" => $"{Hours:00}:{Minutes:00}:{Seconds:00}.{Milliseconds:000}",

			"D" => TotalDays.ToString(),
			"H" => TotalHours.ToString(),
			"M" => TotalMinutes.ToString(),
			"S" => TotalSeconds.ToString(),
			"MS" => TotalMilliseconds.ToString(),

			_ => Value.ToString(format, formatProvider)
		};
	}

	public TimeSpanType Divide(double value)
	{
		return Value.Divide(value);
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return Value.Ticks;
	}

	public override DateTime ToDateTime(IFormatProvider? provider)
	{
		return new DateTime(Value.Ticks);
	}
}