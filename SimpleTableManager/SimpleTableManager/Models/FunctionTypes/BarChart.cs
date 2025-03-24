using System.Text;

namespace SimpleTableManager.Models.FunctionTypes;

public class BarChart<TDataX, TDataY>(IEnumerable<TDataX> xs, IEnumerable<TDataY> ys) :
	ChartBase<TDataX, TDataY>(xs, ys)
	where TDataX: IParsable<TDataX>, IConvertible, IComparable
	where TDataY: IParsable<TDataY>, IConvertible, IComparable
{
	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		//HACK
		int size = 10;
		StringBuilder sb = new();

		foreach (var x in Xs)
		{
			//HACK
			if (x.ToDouble(null) is var xd)
			{
				var i = (int)double.Round(xd);
				var f = size - i;

				sb.Append($"{xd.ToString(format)} " +
				string.Join("", Enumerable.Repeat('█', i)) +
				string.Join("", Enumerable.Repeat('░', f)) +
				"\n");
			}
		}

		return sb.ToString();
	}

	public override int CompareTo(object? obj)
	{
		throw new NotImplementedException();
	}

	public override string ToString()
    {
        return ToString("0.00", null);
    }
}