namespace SimpleTableManager.Models;

[ParseFormat("yyyy-mm-dd", "^(?<y>\\d{4})-(?<m>\\d{2})-(?<d>\\d{2})$")]
[ParseFormat("yyyy.mm.dd", "^(?<y>\\d{4})\\.(?<m>\\d{2})\\.(?<d>\\d{2})$")]
[ParseFormat("yyyy/mm/dd", "^(?<y>\\d{4})/(?<m>\\d{2})/(?<d>\\d{2})$")]
[ParseFormat("dd/mm/yyyy", "^(?<d>\\d{2})/(?<m>\\d{2})/(?<y>\\d{4})$")]
public class DateType(DateOnly dateOnly) :
	TypeBase<DateType, DateOnly>(dateOnly),
	IParsable<DateType>,
	IParsableCore<DateType>,
	IFormattable
{
	public int Year => _value.Year;

	public int Month => _value.Month;

	public int Day => _value.Day;

	public TimeSpan ToTimeSpan() => new(_value.ToDateTime(TimeOnly.MinValue).Ticks);

	public DateType(int year = 1, int month = 1, int day = 1) : this(new(year, month, day)) { }

	public static DateType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		return DateOnly.Parse(args["0"].Value);
	}

	public static implicit operator DateOnly(DateType dateOnly)
	{
		return dateOnly._value;
	}

	public static implicit operator DateType(DateOnly dateOnly)
	{
		return new(dateOnly);
	}

	public DateType Add(TimeSpan timeSpan)
	{
		return DateOnly.FromDateTime(_value.ToDateTime(TimeOnly.MinValue).Add(timeSpan));
	}

	public override DateTime ToDateTime(IFormatProvider? provider)
	{
		return new DateTime(_value, TimeOnly.MinValue);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(format, formatProvider);
	}
}