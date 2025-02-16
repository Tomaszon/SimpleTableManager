namespace SimpleTableManager.Models;

[ParseFormat("true|false", "^(?<true>true)|(?<false>false)$")]
[ParseFormat("yes|no", "^(?<true>yes)|(?<false>no)$")]
public class FormattableBoolean(bool value) : ConvertibleBase<FormattableBoolean>, IParsable<FormattableBoolean>, IParsableCore<FormattableBoolean>, IFormattable
{
	private readonly bool _value = value;

	public static FormattableBoolean ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var name = args.Values.Single(g => g.Success && g.Name != "0").Name;

		return bool.Parse(name);
	}

	public static implicit operator bool(FormattableBoolean value)
	{
		return value._value;
	}

	public static implicit operator FormattableBoolean(bool value)
	{
		return new(value);
	}

	public static FormattableBoolean operator !(FormattableBoolean value)
	{
		return new FormattableBoolean(!value._value);
	}

	public override bool Equals(object? obj)
	{
		if (obj is bool b)
		{
			return _value.Equals(b);
		}
		else if (obj is FormattableBoolean fb && fb is not null)
		{
			return _value.Equals(fb._value);
		}

		return false;
	}

	public override bool ToBoolean(IFormatProvider? provider)
	{
		return this;
	}

	public override int ToInt32(IFormatProvider? provider)
	{
		return this ? 1 : 0;
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return this ? 1 : 0;
	}

	public override double ToDouble(IFormatProvider? provider)
	{
		return this ? 1 : 0;
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return format switch
		{
			"yn" or "y" or "n" => _value ? "Y" : "N",
			"yesno" or "yes" or "no" => _value ? "Yes" : "No",
			"10" or "1" or "0" => _value ? "1" : "0",

			_ => _value.ToString()
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
}