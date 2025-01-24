using System.Numerics;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Power, 1), NamedArgument<double>(ArgumentName.Base, 2)]
[NamedArgument<int>(ArgumentName.Decimals, 2)]
public abstract class NumericFunctionBase<TIn, TOut> : FunctionBase<NumericFunctionOperator, TIn, TOut>
	where TIn : struct, INumber<TIn>, IMinMaxValue<TIn>, TOut
	where TOut : IConvertible
{
	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<TOut>(),

			NumericFunctionOperator.Neg => UnwrappedUnnamedArguments.Select(a => -a).Cast<TOut>(),

			NumericFunctionOperator.Abs => UnwrappedUnnamedArguments.Select(TIn.Abs).Cast<TOut>(),

			NumericFunctionOperator.Sum => Sum(UnwrappedUnnamedArguments).Wrap<TOut>(),

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

			_ => throw GetInvalidOperatorException()
		};
	}

	private IEnumerable<TOut> LogN(IEnumerable<TIn> array, double @base)
	{
		return array.Select(p =>
			double.Round(Math.Log(p.ToDouble(null), @base), GetNamedArgument<int>(ArgumentName.Decimals)).ToType<TOut>());
	}

	private static IEnumerable<TOut> Sqrt(IEnumerable<TIn> array)
	{
		return array.Select(p =>
			Math.Sqrt(p.ToDouble(null)).ToType<TOut>());
	}

	private static IEnumerable<TOut> Power(IEnumerable<TIn> array, int power)
	{
		return array.Select(p =>
			Shared.IndexArray(power).Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= p).ToType<TOut>());
	}

	private static TOut Avg(IEnumerable<TIn> array)
	{
		return (Sum(array) / array.Count().ToType<TIn>()).ToType<TOut>();
	}

	private static TOut Multiply(IEnumerable<TIn> array)
	{
		return array.Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= c).ToType<TOut>();
	}

	protected static TOut Divide(IEnumerable<TIn> array)
	{
		return array.Skip(1).Aggregate(array.First(), (a, c) => a /= c).ToType<TOut>();
	}

	protected static TIn Sum(IEnumerable<TIn> array)
	{
		return array.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
	}

	protected static TOut Sub(IEnumerable<TIn> array)
	{
		return array.Skip(1).Aggregate(array.First(), (a, c) => a -= c).ToType<TOut>();
	}

	private static TOut Min(IEnumerable<TIn> array)
	{
		return (array.Any() ? array.Min() : TIn.MaxValue).ToType<TOut>();
	}

	private static TOut Max(IEnumerable<TIn> array)
	{
		return (array.Any() ? array.Max() : TIn.MinValue).ToType<TOut>();
	}
}