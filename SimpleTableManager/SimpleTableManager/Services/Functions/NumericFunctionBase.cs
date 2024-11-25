using System.Numerics;

namespace SimpleTableManager.Services.Functions;

[NamedArgument(ArgumentName.Power, 1), NamedArgument(ArgumentName.Base, 2)]
public abstract class NumericFunctionBase<TIn, TOut> : FunctionBase<NumericFunctionOperator, TIn, TOut>
	where TIn : struct, INumber<TIn>, IMinMaxValue<TIn>, IConvertible, TOut
{
	public override IEnumerable<TOut> Execute()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => UnwrappedArguments.Cast<TOut>(),

			NumericFunctionOperator.Neg => UnwrappedArguments.Select(a => -a).Cast<TOut>(),

			NumericFunctionOperator.Abs => UnwrappedArguments.Select(TIn.Abs).Cast<TOut>(),

			NumericFunctionOperator.Sum => Sum(UnwrappedArguments).Wrap<TOut>(),

			NumericFunctionOperator.Sub => Sub(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Avg => Avg(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Min => Min(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Max => Max(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Mul => Multiply(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Div => Divide(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Pow => Power(UnwrappedArguments, GetNamedArgument<int>(ArgumentName.Power)),

			NumericFunctionOperator.Sqrt => Sqrt(UnwrappedArguments),

			NumericFunctionOperator.Log2 => LogN(UnwrappedArguments, 2.ToType<TIn>()),

			NumericFunctionOperator.Log10 => LogN(UnwrappedArguments, 10.ToType<TIn>()),

			NumericFunctionOperator.LogE => LogN(UnwrappedArguments, double.E.ToType<TIn>()),

			NumericFunctionOperator.LogN => LogN(UnwrappedArguments, GetNamedArgument<TIn>(ArgumentName.Base)),

			_ => throw GetInvalidOperatorException()
		};
	}

	private static IEnumerable<TOut> LogN(IEnumerable<TIn> array, TIn @base)
	{
		return array.Select(p =>
			Math.Log(p.ToDouble(null), @base.ToDouble(null)).ToType<TOut>());
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