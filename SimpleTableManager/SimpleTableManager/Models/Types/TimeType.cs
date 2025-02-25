namespace SimpleTableManager.Models.Types;

[ParseFormat("hh:mm:ss.fff", "^(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class TimeType(TimeOnly timeOnly) :
	TypeBase<TimeType, TimeOnly>(timeOnly),
	IParsable<TimeType>,
	IParsableCore<TimeType>
{
	public int Hour => _value.Hour;

	public int Minute => _value.Minute;

	public int Second => _value.Second;

	public int Millisecond => _value.Millisecond;

	public TimeSpan ToTimeSpan() => _value.ToTimeSpan();

	public TimeType(int hour = 0, int minute = 0, int second = 0, int millisecond = 0) : this(new(hour, minute, second, millisecond)) { }

	public static TimeType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return TimeOnly.Parse(args["0"].Value);
	}

	public static implicit operator TimeOnly(TimeType timeOnly)
	{
		return timeOnly._value;
	}

	public static implicit operator TimeType(TimeOnly timeOnly)
	{
		return new(timeOnly);
	}

	public TimeType Add(TimeSpan timeSpan)
	{
		return _value.Add(timeSpan);
	}

	public override DateTime ToDateTime(IFormatProvider? provider)
	{
		return new DateTime(DateOnly.MinValue, _value);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(format, formatProvider);
	}
}