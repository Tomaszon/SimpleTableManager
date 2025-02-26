namespace SimpleTableManager.Models.Types;

[ParseFormat("value", "^.+$")]
public class StringType(string value) :
	TypeBase<StringType, string>(value),
	IParsable<StringType>,
	IParsableCore<StringType>
{
	public IntegerType Length => _value.Length;

	public static StringType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var value = args.Values.Single(g => g.Success && g.Name != "0").Value;

		return value;
	}

	public static implicit operator string(StringType value)
	{
		return value._value;
	}

	public static implicit operator StringType(string value)
	{
		return new(value);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(formatProvider);
	}

	public CharacterType[] ToArray()
	{
		return [.. _value.Select(c => (CharacterType)c)];
	}

	public StringType[] Split(string separator)
	{
		return [.. _value.Split(separator).Select(s => (StringType)s)];
	}

	public StringType Trim(char trim)
	{
		return _value.Trim(trim);
	}

	public static StringType Concat(IEnumerable<IType> array)
	{
		return string.Concat(array);
	}
}