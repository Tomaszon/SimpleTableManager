namespace SimpleTableManager.Models;

[ParseFormat("hh:mm:ss.fff", "^(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class ConvertibleTimeOnly(int hour = 0, int minute = 0, int second = 0, int millisecond = 0) :
	ConvertibleBase<ConvertibleTimeOnly>,
	IParsable<ConvertibleTimeOnly>,
	IParsableCore<ConvertibleTimeOnly>,
	IFormattable,
	IComparable
{
	private readonly TimeOnly _value = new(hour, minute, second, millisecond);

	public int Hour => _value.Hour;

	public int Minute => _value.Minute;

	public int Second => _value.Second;

	public int Millisecond => _value.Millisecond;

	public TimeSpan ToTimeSpan() => _value.ToTimeSpan();

	public ConvertibleTimeOnly(TimeOnly timeOnly) : this(timeOnly.Hour, timeOnly.Minute, timeOnly.Second, timeOnly.Millisecond) { }

	public static ConvertibleTimeOnly ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return TimeOnly.Parse(args["0"].Value);
	}

	public static implicit operator TimeOnly(ConvertibleTimeOnly timeOnly)
	{
		return timeOnly._value;
	}

	public static implicit operator ConvertibleTimeOnly(TimeOnly timeOnly)
	{
		return new(timeOnly);
	}

	public ConvertibleTimeOnly Add(TimeSpan timeSpan)
	{
		return _value.Add(timeSpan);
	}

	public override bool Equals(object? obj)
	{
		if (obj is TimeOnly t)
		{
			return _value.Equals(t);
		}
		else if (obj is ConvertibleTimeOnly cto && cto is not null)
		{
			return _value.Equals(cto._value);
		}

		return false;
	}

	public override DateTime ToDateTime(IFormatProvider? provider)
	{
		return new DateTime(DateOnly.MinValue, _value);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(format, formatProvider);
	}

	public override string ToString()
	{
		return _value.ToString();
	}

	public int CompareTo(object? obj)
	{
		if (obj is ConvertibleTimeOnly cto && cto is not null)
		{
			return _value.CompareTo(cto._value);
		}

		return _value.CompareTo(obj);
	}

	[ExcludeFromCodeCoverage]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}