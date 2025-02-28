using System.Numerics;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Power, 1), NamedArgument<double>(ArgumentName.Base, 2)]
[NamedArgument<int>(ArgumentName.Decimals, 2)]
public abstract class NumericFunctionBase<TIn> :
	FunctionBase<NumericFunctionOperator, TIn, object>
	where TIn : struct, INumber<TIn>, IMinMaxValue<TIn>, IConvertible
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<object>(),

			NumericFunctionOperator.Neg => UnwrappedUnnamedArguments.Select(a => -a).Cast<object>(),

			NumericFunctionOperator.Abs => UnwrappedUnnamedArguments.Select(TIn.Abs).Cast<object>(),

			NumericFunctionOperator.Sum => Sum(UnwrappedUnnamedArguments).Wrap<object>(),

			NumericFunctionOperator.Sub => Sub(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Avg => Avg(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Min => Min(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Max => Max(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Mul => Multiply(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Div => Divide(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Pow => Power(UnwrappedUnnamedArguments, GetNamedArgument<int>(ArgumentName.Power)),

			NumericFunctionOperator.Sqrt => Sqrt(UnwrappedUnnamedArguments),

			NumericFunctionOperator.Log2 => LogN(UnwrappedUnnamedArguments, 2),

			NumericFunctionOperator.Log10 => LogN(UnwrappedUnnamedArguments, 10),

			NumericFunctionOperator.LogE => LogN(UnwrappedUnnamedArguments, double.E),

			NumericFunctionOperator.LogN => LogN(UnwrappedUnnamedArguments, GetNamedArgument<double>(ArgumentName.Base)),
			
			NumericFunctionOperator.Greater => Greater().Wrap().Cast<object>(),

			NumericFunctionOperator.Less => Less().Wrap().Cast<object>(),

			NumericFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap().Cast<object>(),

			NumericFunctionOperator.LessOrEquals => LessOrEquals().Wrap().Cast<object>(),

			NumericFunctionOperator.Equals => Equals().Wrap().Cast<object>(),

			NumericFunctionOperator.NotEquals => NotEquals().Wrap().Cast<object>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private IEnumerable<object> LogN(IEnumerable<TIn> array, double @base)
	{
		return array.Select(p =>
			(object)double.Round(Math.Log(p.ToDouble(null), @base), GetNamedArgument<int>(ArgumentName.Decimals)));
	}

	private static IEnumerable<object> Sqrt(IEnumerable<TIn> array)
	{
		return array.Select(p =>
			(object)Math.Sqrt(p.ToDouble(null)));
	}

	private static IEnumerable<object> Power(IEnumerable<TIn> array, int power)
	{
		return array.Select(p =>
			(object)Shared.IndexArray(power).Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= p));
	}

	private static object Avg(IEnumerable<TIn> array)
	{
		return Sum(array) / array.Count().ToType<TIn>();
	}

	private static object Multiply(IEnumerable<TIn> array)
	{
		return array.Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= c);
	}

	protected static object Divide(IEnumerable<TIn> array)
	{
		return array.Skip(1).Aggregate(array.First(), (a, c) => a /= c);
	}

	protected static TIn Sum(IEnumerable<TIn> array)
	{
		return array.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
	}

	protected static object Sub(IEnumerable<TIn> array)
	{
		return array.Skip(1).Aggregate(array.First(), (a, c) => a -= c);
	}

	private static object Min(IEnumerable<TIn> array)
	{
		return (array.Any() ? array.Min() : TIn.MaxValue);
	}

	private static object Max(IEnumerable<TIn> array)
	{
		return (array.Any() ? array.Max() : TIn.MinValue);
	}
}