
namespace SimpleTableManager.Models;

public class ConvertibleDateOnly(DateOnly value) : ConvertibleBase<ConvertibleDateOnly>, IParsable<ConvertibleDateOnly>, IParseCore<ConvertibleDateOnly>
{
	public DateOnly Value { get; set; } = value;

	public static ConvertibleDateOnly ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		throw new NotImplementedException();
	}

	public static implicit operator ConvertibleDateOnly(DateOnly dateOnly)
	{
		return new(dateOnly);
	}
}