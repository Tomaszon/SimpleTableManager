namespace SimpleTableManager.Models.Types;

[ParseFormat("d", "^(?<d>\\d+)$")]
[ParseFormat("d.hh", "^(?<d>\\d+)\\.(?<h>\\d{2})$")]
[ParseFormat("d.hh:mm:ss.fff", "^((?<d>\\d+)\\.)?(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class TimeSpanType(TimeSpan timeSpan) :
	TypeBase<TimeSpanType, TimeSpan>(timeSpan),
	IParsable<TimeSpanType>,
	IParsableCore<TimeSpanType>
{
	public IntegerType Days => _value.Days;

	public FractionType TotalDays => _value.TotalDays;

	public IntegerType Hours => _value.Hours;

	public FractionType TotalHours => _value.TotalHours;

	public IntegerType Minutes => _value.Minutes;

	public FractionType TotalMinutes => _value.TotalMinutes;

	public IntegerType Seconds => _value.Seconds;

	public FractionType TotalSeconds => _value.TotalSeconds;

	public IntegerType Milliseconds => _value.Milliseconds;

	public FractionType TotalMilliseconds => _value.TotalMilliseconds;

	public TimeSpanType(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) : this(new(days, hours, minutes, seconds, milliseconds)) { }

	public static TimeSpanType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return TimeSpan.Parse(args["0"].Value, formatProvider);
	}

	public static implicit operator TimeSpan(TimeSpanType timeSpan)
	{
		return timeSpan._value;
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

			_ => _value.ToString(format, formatProvider)
		};
	}

	public TimeSpanType Divide(double value)
	{
		return _value.Divide(value);
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return _value.Ticks;
	}

	public override DateTime ToDateTime(IFormatProvider? provider)
	{
		return new DateTime(_value.Ticks);
	}
}