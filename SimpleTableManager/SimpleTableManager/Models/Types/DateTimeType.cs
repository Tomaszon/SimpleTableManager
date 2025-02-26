namespace SimpleTableManager.Models.Types;

[ParseFormat("yyyy-mm-ddThh:mm:ss.fff",
	"^(?<y>\\d{4})-(?<m>\\d{2})-(?<d>\\d{2})( |T)(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
[ParseFormat("yyyy.mm.ddThh:mm:ss.fff",
	"^(?<y>\\d{4})\\.(?<m>\\d{2})\\.(?<d>\\d{2})( |T)(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
[ParseFormat("yyyy/mm/ddThh:mm:ss.fff",
	"^(?<y>\\d{4})/(?<m>\\d{2})/(?<d>\\d{2})( |T)(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
[ParseFormat("dd/mm/yyyyThh:mm:ss.fff",
	"^(?<d>\\d{2})/(?<m>\\d{2})/(?<y>\\d{4})( |T)(?<h>\\d{2}):(?<m>\\d{2})(:(?<s>\\d{2})(\\.(?<f>\\d{1,3}))?)?$")]
public class DateTimeType(DateTime value) :
	TypeBase<DateTimeType, DateTime>(value),
	IParsable<DateTimeType>,
	IParsableCore<DateTimeType>
{
	public IntegerType Year => _value.Year;

	public IntegerType Month => _value.Month;

	public IntegerType Day => _value.Day;

	public IntegerType Hour => _value.Hour;

	public IntegerType Minute => _value.Minute;

	public IntegerType Second => _value.Second;

	public IntegerType Millisecond => _value.Millisecond;

	public TimeSpanType TimeOfDay => _value.TimeOfDay;

	public IntegerType Ticks => _value.Ticks;

	public DateTimeType(int year = 1, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0, int millisecond = 0) : this(new DateTime(year, month, day, hour, minute, second, millisecond)) { }

	public static DateTimeType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var value = args.Values.Single(g => g.Success && g.Name != "0").Value;

		return DateTime.Parse(value, formatProvider);
	}

	public static implicit operator DateTime(DateTimeType value)
	{
		return value._value;
	}

	public static implicit operator DateTimeType(DateTime value)
	{
		return new(value);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(formatProvider);
	}

	public DateTimeType Add(TimeSpanType timeSpan)
	{
		return _value.Add(timeSpan);
	}
}