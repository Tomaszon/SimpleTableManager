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

			NumericFunctionOperator.Sub => Sub(Arguments).Wrap<TOut>(),

			NumericFunctionOperator.Avg => Avg(Arguments/*.Union(ReferenceArguments)*/).Wrap<TOut>(),

			NumericFunctionOperator.Min => Min(Arguments).Wrap<TOut>(),
			//new[] { Min(Arguments), Min(ReferenceArguments) }.Min().Wrap<TOut>(),

			NumericFunctionOperator.Max => Max(Arguments).Wrap<TOut>(),
			//new[] { Max(Arguments), Max(ReferenceArguments) }.Max().Wrap<TOut>(),

			NumericFunctionOperator.Mul => Multiply(Arguments).Wrap<TOut>(),

			NumericFunctionOperator.Div => Divide(Arguments).Wrap<TOut>(),

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

	private static TIn Avg(IEnumerable<TIn> array)
	{
		return Sum(array) / TIn.Parse(array.Count().ToString(), NumberStyles.Integer, null);
	}

	private static TIn Multiply(IEnumerable<TIn> array)
	{
		return array.Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= c);
	}

	protected static TIn Divide(IEnumerable<TIn> array)
	{
		return array.Skip(1).Aggregate(array.First(), (a, c) => a /= c);
	}

	protected static TIn Sum(IEnumerable<TIn> array)
	{
		return array.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
	}

	protected static TIn Sub(IEnumerable<TIn> array)
	{
		return array.Skip(1).Aggregate(array.First(), (a, c) => a -= c);
	}

	private static TIn Min(IEnumerable<TIn> array)
	{
		return array.Any() ? array.Min() : TIn.MaxValue;
	}

	private static TIn Max(IEnumerable<TIn> array)
	{
		return array.Any() ? array.Max() : TIn.MinValue;
	}
}