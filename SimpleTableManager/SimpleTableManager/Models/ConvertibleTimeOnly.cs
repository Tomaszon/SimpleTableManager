namespace SimpleTableManager.Models;

[ParseFormat("hh:mm:ss.fff", "^(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
//TODO implement IFormattable
public class ConvertibleTimeOnly(TimeOnly value) : ConvertibleBase<ConvertibleTimeOnly>, IParsable<ConvertibleTimeOnly>, IParsableCore<ConvertibleTimeOnly>
{
	[JsonProperty]
	private readonly TimeOnly _value = value;

	public int Hour => _value.Hour;

	public int Minute => _value.Minute;

	public int Second => _value.Second;

	public int Millisecond => _value.Millisecond;

	public TimeSpan ToTimeSpan() => _value.ToTimeSpan();

	public static ConvertibleTimeOnly ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return TimeOnly.Parse(args["0"].Value);
	}

	public static implicit operator ConvertibleTimeOnly(TimeOnly timeOnly)
	{
		return new(timeOnly);
	}

	public override bool Equals(object? obj)
	{
		if (obj is ConvertibleTimeOnly cto && cto is not null)
		{
			return _value.Equals(cto._value);
		}

		return false;
	}

	public override string ToString()
	{
		return _value.ToString();
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}