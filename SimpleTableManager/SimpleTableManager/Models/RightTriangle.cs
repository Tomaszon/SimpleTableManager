namespace SimpleTableManager.Models;

public class RightTriangle : Shape2Sized2dBase<RightTriangle>, IParsable<RightTriangle>
{
	public override double Area => Size1 * Size2 / 2;

	public override double Perimeter =>
		Size1 + Size2 + Math.Sqrt(Size1 * Size1 + Size2 * Size2);

	public RightTriangle() { }

	public RightTriangle(double size1, double size2) : base(size1, size2) { }
}
