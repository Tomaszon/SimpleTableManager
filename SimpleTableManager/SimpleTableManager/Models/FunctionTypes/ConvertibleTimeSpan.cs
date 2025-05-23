namespace SimpleTableManager.Models.FunctionTypes;

[ParseFormat("d", "^(?<d>\\d+)$")]
[ParseFormat("d.hh", "^(?<d>\\d+)\\.(?<h>\\d{2})$")]
[ParseFormat("d.hh:mm:ss.fff", "^((?<d>\\d+)\\.)?(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class ConvertibleTimeSpan(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) :
	ConvertibleBase<ConvertibleTimeSpan>,
	IParsable<ConvertibleTimeSpan>,
	IParsableCore<ConvertibleTimeSpan>,
	IFormattable,
	IComparable
{
	[JsonProperty]
	private readonly TimeSpan _value = new(days, hours, minutes, seconds, milliseconds);

	public int Days => _value.Days;

	public double TotalDays => _value.TotalDays;

	public int Hours => _value.Hours;

	public double TotalHours => _value.TotalHours;

	public int Minutes => _value.Minutes;

	public double TotalMinutes => _value.TotalMinutes;

	public int Seconds => _value.Seconds;

	public double TotalSeconds => _value.TotalSeconds;

	public int Milliseconds => _value.Milliseconds;

	public double TotalMilliseconds => _value.TotalMilliseconds;

	public ConvertibleTimeSpan(TimeSpan timeSpan) : this(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds) { }

	public static ConvertibleTimeSpan ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return TimeSpan.Parse(args["0"].Value);
	}

	public static implicit operator TimeSpan(ConvertibleTimeSpan timeSpan)
	{
		return timeSpan._value;
	}

	public static implicit operator ConvertibleTimeSpan(TimeSpan timeSpan)
	{
		return new(timeSpan);
	}

	public override bool Equals(object? obj)
	{
		if (obj is TimeSpan s)
		{
			return _value.Equals(s);
		}
		else if (obj is ConvertibleTimeSpan cts && cts is not null)
		{
			return _value.Equals(cts._value);
		}

		return false;
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
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

	public ConvertibleTimeSpan Divide(double value)
	{
		return _value.Divide(value);
	}

	public ConvertibleTimeSpan Multiply(double value)
	{
		return _value.Multiply(value);
	}

	public override string ToString()
	{
		return _value.ToString();
	}

	public int CompareTo(object? obj)
	{
		if (obj is ConvertibleTimeSpan cts && cts is not null)
		{
			return _value.CompareTo(cts._value);
		}

		return _value.CompareTo(obj);
	}

	[ExcludeFromCodeCoverage]
	public override int GetHashCode()
	{
		return base.GetHashCode();
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