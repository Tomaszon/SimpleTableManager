using System.Numerics;
using System.Text;

namespace SimpleTableManager.Models;

[ParseFormat("{x1;x2;...xn}{y1;y2...yn}", "{(?<x>(.*;)*.*)}{(?<y>(.*;)*.*)}")]
public abstract class ChartBase<T>(IEnumerable<IConvertible> xs, IEnumerable<IConvertible> ys) :
	ConvertibleBase<T>,
	IParsableCore<T>,
	IFormattable,
	IChart
	where T : ChartBase<T>, IParsable<T>
{
	public IConvertible[] Xs { get; set; } = [.. xs];

	public IConvertible[] Ys { get; set; } = [.. ys];

	public static T ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var xs = args["x"].Value.Split(';');
		var ys = args["y"].Value.Split(';');

		return (T)Activator.CreateInstance(typeof(T), xs, ys)!;
	}

	public abstract string ToString(string? format, IFormatProvider? formatProvider);
}

public interface IChart : IConvertible
{
	public IConvertible[] Xs { get; set; }

	public IConvertible[] Ys { get; set; }
}

public class RawChart(IEnumerable<IConvertible> xs, IEnumerable<IConvertible> ys) :
	ChartBase<RawChart>(xs, ys),
	IParsable<RawChart>
{
	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return $"{{{string.Join(';', Xs.Select(v => v.ToString()))}}}->{{{string.Join(';', Ys.Select(v => v.ToString()))}}}";
	}

	public override string ToString()
	{
		return $"{{{string.Join(';', Xs.Select(v => v.ToString()))}}}->{{{string.Join(';', Ys.Select(v => v.ToString()))}}}";
	}
}

public class BarChart(IEnumerable<IConvertible> xs, IEnumerable<IConvertible> ys) :
	ChartBase<BarChart>(xs, ys),
	IParsable<BarChart>
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

    public override string ToString()
    {
        return ToString("0.00", null);
    }
}