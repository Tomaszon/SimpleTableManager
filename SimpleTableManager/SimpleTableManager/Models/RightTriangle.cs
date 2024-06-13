namespace SimpleTableManager.Models;

public class RightTriangle : Shape2Sized2dBase<RightTriangle>, IParsable<RightTriangle>
{
	public override decimal Area => Size1 * Size2 / 2;

	public override decimal Perimeter =>
		Size1 + Size2 + Math.Sqrt((Size1 * Size1 + Size2 * Size2).ToType<double>()).ToType<decimal>();

	public static RightTriangle Parse(string value, IFormatProvider? provider)
	{
		return ParseWrapper(value);
	}
}
