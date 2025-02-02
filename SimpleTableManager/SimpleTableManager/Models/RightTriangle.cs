namespace SimpleTableManager.Models;

[ParseFormat("T[size1*size2] (fractions)", "^T\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("T[size1xsize2] (fractions)", "^T\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("{0}[size1*size2] (fractions)", "^Arg0\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("{0}[size1xsize2] (fractions)", "^Arg0\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$", [nameof(RightTriangle)])]
public class RightTriangle(double size1, double size2) : Shape2Sized2dBase<RightTriangle>(size1, size2), IParsable<RightTriangle>
{
	public override double Area => Size1 * Size2 / 2;

	public override double Perimeter =>
		Size1 + Size2 + Math.Sqrt(Size1 * Size1 + Size2 * Size2);
}
