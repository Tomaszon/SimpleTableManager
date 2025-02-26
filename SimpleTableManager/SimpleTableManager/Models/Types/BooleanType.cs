namespace SimpleTableManager.Models.Types;

[ParseFormat("true|false", "^(?<true>true)|(?<false>false)$")]
[ParseFormat("yes|no", "^(?<true>yes)|(?<false>no)$")]
public class BooleanType(bool value) :
	TypeBase<BooleanType, bool>(value),
	IParsable<BooleanType>,
	IParsableCore<BooleanType>
{
	public static BooleanType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var name = args.Values.Single(g => g.Success && g.Name != "0").Name;

		return bool.Parse(name);
	}

	public static implicit operator bool(BooleanType value)
	{
		return value._value;
	}

	public static implicit operator BooleanType(bool value)
	{
		return new(value);
	}

	public static BooleanType operator !(BooleanType value)
	{
		return new BooleanType(!value._value);
	}

	public override bool ToBoolean(IFormatProvider? provider)
	{
		return _value;
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return this ? 1 : 0;
	}

	public override double ToDouble(IFormatProvider? provider)
	{
		return this ? 1 : 0;
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return format switch
		{
			"yn" or
			"y" or
			"n" => _value ? "Y" : "N",
			"yesno" or
			"yes" or
			"no" => _value ? "Yes" : "No",
			"10" or
			"1" or
			"0" => _value ? "1" : "0",

			_ => _value.ToString()
		};
	}
}