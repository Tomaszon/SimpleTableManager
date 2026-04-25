namespace SimpleTableManager.Models.FunctionTypes;

public enum TrileanValue
{
	True = 1,
	Unknown = 0,
	False = -1
}

[ParseFormat("true|unknown|false", "^(?<3>true)|(?<2>unknown)|(?<1>false)$")]
[ParseFormat("yes|maybe|no", "^(?<3>yes)|(?<2>maybe)|(?<1>no)$")]
[ParseFormat("1|0|-1", "^(?<3>1)|(?<2>0)|(?<1>-1)$")]
public class Trilean(TrileanValue value) :
	ConvertibleBase<Trilean>,
	IParsable<Trilean>,
	IParsableCore<Trilean>,
	IComparable,
	IFormattable
{
	private readonly TrileanValue _value = value;

	public static Trilean ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var name = args.Values.Single(g => g.Success && g.Name != "0").Name;

		return int.Parse(name) - 2;
	}

	public static implicit operator int(Trilean value)
	{
		return (int)value._value;
	}

	public static implicit operator Trilean(int value)
	{
		return new((TrileanValue)value);
	}

	public static implicit operator bool(Trilean value)
	{
		return value.ToBoolean(null);
	}

	public static implicit operator Trilean(bool value)
	{
		return new((TrileanValue)Convert.ToInt32(value));
	}

	public static Trilean operator !(Trilean value)
	{
		return new Trilean((TrileanValue)(-(int)value._value));
	}

	public new Trilean Equals(object? obj)
	{
		if (obj is Trilean t)
		{
			var mul = (int)t._value * (int)_value;

			return mul switch
			{
				> 0 => 1,
				< 0 => -1,
				_ => 0
			};
		}
		else if (obj is bool b)
		{
			return _value > 0 && b || _value < 0 && !b;
		}

		return -1;
	}

	public override bool ToBoolean(IFormatProvider? provider)
	{
		return _value switch
		{
			> 0 => true,
			< 0 => false,
			_ => throw new OverflowException(),
		};
	}

	public override int ToInt32(IFormatProvider? provider)
	{
		return (int)_value;
	}

	public override long ToInt64(IFormatProvider? provider)
	{
		return (long)_value;
	}

	public override double ToDouble(IFormatProvider? provider)
	{
		return (double)_value;
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return format switch
		{
			"tf" or
			"t" or
			"f" or
			"u" => _value switch
			{
				> 0 => "T",
				< 0 => "F",
				_ => "U"
			},
			"yn" or
			"y" or
			"n" or
			"m" => _value switch
			{
				> 0 => "Y",
				< 0 => "N",
				_ => "M"
			},
			"yesno" or
			"yes" or
			"no" or
			"maybe" => _value switch
			{
				> 0 => "Yes",
				< 0 => "No",
				_ => "Maybe"
			},
			"10" or
			"1" or
			"0" or
			"-1" => _value switch
			{
				> 0 => "1",
				< 0 => "-1",
				_ => "0",
			},

			_ => ToString()
		};
	}

	public override string ToString()
	{
		return _value switch
		{
			> 0 => "True",
			< 0 => "False",
			_ => "Unknown"
		};
	}

	public int CompareTo(object? obj)
	{
		if (obj is Trilean t && t is not null)
		{
			return _value.CompareTo(t._value);
		}

		return _value.CompareTo(obj);
	}

	[ExcludeFromCodeCoverage]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}