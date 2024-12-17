namespace SimpleTableManager.Models;

public class Ellipse : Shape2Sized2dBase<Ellipse>, IParsable<Ellipse>
{
	public override double Area => Size1 * Size2 * Math.PI;

	public override double Perimeter => Math.PI * Math.Sqrt(2 * (Size1 * Size1 + Size2 * Size2));

	public Ellipse() { }

	public Ellipse(double size1, double size2) : base(size1, size2) { }
}
