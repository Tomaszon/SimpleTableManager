namespace SimpleTableManager.Models;

public class Ellipse(double size1 = 0, double size2 = 0) : Shape2Sized2dBase<Ellipse>(size1, size2), IParsable<Ellipse>
{
	public override double Area => Size1 * Size2 * Math.PI;

	public override double Perimeter => Math.PI * Math.Sqrt(2 * (Size1 * Size1 + Size2 * Size2));
}
