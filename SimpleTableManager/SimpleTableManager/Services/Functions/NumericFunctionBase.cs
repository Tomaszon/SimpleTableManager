using System.Numerics;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Power, 1), NamedArgument<double>(ArgumentName.Base, 2)]
[NamedArgument<int>(ArgumentName.Decimals, 2)]
public abstract class NumericFunctionBase<TIn, TOut> : FunctionBase<NumericFunctionOperator, TIn, TOut>
	where TIn : struct, INumber<TIn>, IMinMaxValue<TIn>, IConvertible, TOut
{
	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => ConvertedUnwrappedArguments.Cast<TOut>(),

			NumericFunctionOperator.Neg => ConvertedUnwrappedArguments.Select(a => -a).Cast<TOut>(),

			NumericFunctionOperator.Abs => ConvertedUnwrappedArguments.Select(TIn.Abs).Cast<TOut>(),

			NumericFunctionOperator.Sum => Sum(ConvertedUnwrappedArguments).Wrap<TOut>(),

			NumericFunctionOperator.Sub => Sub(ConvertedUnwrappedArguments).Wrap(),

			NumericFunctionOperator.Avg => Avg(ConvertedUnwrappedArguments).Wrap(),

			NumericFunctionOperator.Min => Min(ConvertedUnwrappedArguments).Wrap(),

			NumericFunctionOperator.Max => Max(ConvertedUnwrappedArguments).Wrap(),

			NumericFunctionOperator.Mul => Multiply(ConvertedUnwrappedArguments).Wrap(),

			NumericFunctionOperator.Div => Divide(ConvertedUnwrappedArguments).Wrap(),

			NumericFunctionOperator.Pow => Power(ConvertedUnwrappedArguments, GetNamedArgument<int>(ArgumentName.Power)),

			NumericFunctionOperator.Sqrt => Sqrt(ConvertedUnwrappedArguments),

			NumericFunctionOperator.Log2 => LogN(ConvertedUnwrappedArguments, 2),

			NumericFunctionOperator.Log10 => LogN(ConvertedUnwrappedArguments, 10),

			NumericFunctionOperator.LogE => LogN(ConvertedUnwrappedArguments, double.E),

			NumericFunctionOperator.LogN => LogN(ConvertedUnwrappedArguments, GetNamedArgument<double>(ArgumentName.Base)),

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