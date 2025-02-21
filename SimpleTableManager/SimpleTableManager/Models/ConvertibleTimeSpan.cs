
namespace SimpleTableManager.Models;

[ParseFormat("d", "^(?<d>\\d+)$")]
[ParseFormat("d.hh", "^(?<d>\\d+)\\.(?<h>\\d{2})$")]
[ParseFormat("d.hh:mm:ss.fff", "^((?<d>\\d+)\\.)?(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class ConvertibleTimeSpan(TimeSpan value) :
	ConvertibleBase<ConvertibleTimeSpan>,
	IParsable<ConvertibleTimeSpan>,
	IParsableCore<ConvertibleTimeSpan>,
	IFormattable
{
	[JsonProperty]
	private readonly TimeSpan _value = value;

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

	public override string ToString()
	{
		return _value.ToString();
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