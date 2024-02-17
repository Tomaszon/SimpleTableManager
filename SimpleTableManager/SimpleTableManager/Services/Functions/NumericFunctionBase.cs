using System.Globalization;
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
			NumericFunctionOperator.Const => Arguments.Cast<TOut>(),

			NumericFunctionOperator.Neg => Arguments.Select(a => -a).Cast<TOut>(),

			NumericFunctionOperator.Abs => Arguments.Select(TIn.Abs).Cast<TOut>(),

			NumericFunctionOperator.Sum => Sum(Arguments/*.Union(ReferenceArguments)*/).Wrap<TOut>(),

			NumericFunctionOperator.Sub => Sub(Arguments).Wrap(),

			NumericFunctionOperator.Avg => Avg(Arguments/*.Union(ReferenceArguments)*/).Wrap(),

			NumericFunctionOperator.Min => Min(Arguments).Wrap(),
			//new[] { Min(Arguments), Min(ReferenceArguments) }.Min().Wrap<TOut>(),

			NumericFunctionOperator.Max => Max(Arguments).Wrap(),
			//new[] { Max(Arguments), Max(ReferenceArguments) }.Max().Wrap<TOut>(),

			NumericFunctionOperator.Mul => Multiply(Arguments).Wrap(),

			NumericFunctionOperator.Div => Divide(Arguments).Wrap(),

			NumericFunctionOperator.Pow => Power(Arguments, GetNamedArgument<int>(ArgumentName.Power)),

			NumericFunctionOperator.Sqrt => Sqrt(Arguments),

			NumericFunctionOperator.Log2 => LogN(Arguments, 2.ToType<TIn>()),

			NumericFunctionOperator.Log10 => LogN(Arguments, 10.ToType<TIn>()),

			NumericFunctionOperator.LogE => LogN(Arguments, double.E.ToType<TIn>()),

			NumericFunctionOperator.LogN => LogN(Arguments, GetNamedArgument<TIn>(ArgumentName.Base)),

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
		return  (Sum(array) / array.Count().ToType<TIn>()).ToType<TOut>();
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