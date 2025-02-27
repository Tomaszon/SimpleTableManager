namespace SimpleTableManager.Models.Types;

[ParseFormat("value", "^.+$")]
public class StringType(string value) :
	TypeBase<StringType, string>(value),
	IParsable<StringType>,
	IParsableCore<StringType>
{
	public IntegerType Length => Value.Length;

	public static StringType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var value = args.Values.Single(g => g.Success && g.Name != "0").Value;

		return value;
	}

	public static implicit operator string(StringType value)
	{
		return value.Value;
	}

	public static implicit operator StringType(string value)
	{
		return new(value);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return Value.ToString(formatProvider);
	}

	public CharacterType[] ToArray()
	{
		return [.. Value.Select(c => (CharacterType)c)];
	}

	public StringType[] Split(string separator)
	{
		return [.. Value.Split(separator).Select(s => (StringType)s)];
	}

	public StringType Trim(char trim)
	{
		return Value.Trim(trim);
	}

	public static StringType Concat(IEnumerable<IType> array)
	{
		return string.Concat(array);
	}
}