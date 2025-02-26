namespace SimpleTableManager.Models.Types;

[ParseFormat("character", "^.$")]
public class CharacterType(char c) :
	TypeBase<CharacterType, char>(c),
	IParsable<CharacterType>,
	IParsableCore<CharacterType>
{
	public static CharacterType ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var value = args.Values.Single(g => g.Success && g.Name != "0").Value;

		return char.Parse(value);
	}

	public static implicit operator char(CharacterType value)
	{
		return value._value;
	}

	public static implicit operator CharacterType(char value)
	{
		return new(value);
	}

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return _value.ToString(formatProvider);
	}
}