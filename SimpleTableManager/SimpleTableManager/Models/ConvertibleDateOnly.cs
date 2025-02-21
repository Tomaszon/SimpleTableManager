namespace SimpleTableManager.Models;

[ParseFormat("yyyy-mm-dd", "^(?<y>\\d{4})-(?<m>\\d{2})-(?<d>\\d{2})$")]
[ParseFormat("yyyy.mm.dd", "^(?<y>\\d{4})\\.(?<m>\\d{2})\\.(?<d>\\d{2})$")]
[ParseFormat("yyyy/mm/dd", "^(?<y>\\d{4})/(?<m>\\d{2})/(?<d>\\d{2})$")]
[ParseFormat("dd/mm/yyyy", "^(?<d>\\d{2})/(?<m>\\d{2})/(?<y>\\d{4})$")]
public class ConvertibleDateOnly(DateOnly value) :
	ConvertibleBase<ConvertibleDateOnly>,
	IParsable<ConvertibleDateOnly>,
	IParsableCore<ConvertibleDateOnly>,
	IFormattable
{
	private readonly DateOnly _value = value;

	public int Year => _value.Year;

	public int Month => _value.Month;

	public int Day => _value.Day;

	public TimeSpan ToTimeSpan() => new(_value.ToDateTime(TimeOnly.MinValue).Ticks);

	public static ConvertibleDateOnly ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return DateOnly.Parse(args["0"].Value);
	}

	public static implicit operator DateOnly(ConvertibleDateOnly dateOnly)
	{
		return dateOnly._value;
	}

	public static implicit operator ConvertibleDateOnly(DateOnly dateOnly)
	{
		return new(dateOnly);
	}

	public ConvertibleDateOnly Add(TimeSpan timeSpan)
	{
		return DateOnly.FromDateTime(_value.ToDateTime(TimeOnly.MinValue).Add(timeSpan));
	}

	public override bool Equals(object? obj)
	{
		if (obj is DateOnly d)
		{
			return _value.Equals(d);
		}
		else if (obj is ConvertibleDateOnly cdo && cdo is not null)
		{
			return _value.Equals(cdo._value);
		}

		return false;
	}

	public override DateTime ToDateTime(IFormatProvider? provider)
	{
		return new DateTime(_value, TimeOnly.MinValue);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(format, formatProvider);
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
}