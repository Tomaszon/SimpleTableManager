using System.Globalization;
using System.Numerics;

namespace SimpleTableManager.Services.Functions;

public abstract class NumericFunctionBase<TIn, TOut> : FunctionBase<NumericFunctionOperator, TIn, TOut>
	where TIn : struct, INumber<TIn>, IMinMaxValue<TIn>, TOut
{
	public override IEnumerable<TOut> Execute()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => Arguments.ToArray().Cast<TOut>(),

			NumericFunctionOperator.Neg => Arguments.Select(a => -a).Cast<TOut>(),

			NumericFunctionOperator.Abs => Arguments.Select(a => TIn.Abs(a)).Cast<TOut>(),

			NumericFunctionOperator.Sum => Sum(Arguments/*.Union(ReferenceArguments)*/).Wrap<TOut>(),

			NumericFunctionOperator.Sub => Sub(Arguments).Wrap<TOut>(),

			NumericFunctionOperator.Avg => Avg(Arguments/*.Union(ReferenceArguments)*/).Wrap<TOut>(),

			NumericFunctionOperator.Min => Min(Arguments).Wrap<TOut>(),
			//new[] { Min(Arguments), Min(ReferenceArguments) }.Min().Wrap<TOut>(),

			NumericFunctionOperator.Max => Max(Arguments).Wrap<TOut>(),
			//new[] { Max(Arguments), Max(ReferenceArguments) }.Max().Wrap<TOut>(),

			NumericFunctionOperator.Mul => Multiply(Arguments).Wrap<TOut>(),

			NumericFunctionOperator.Div => Divide(Arguments).Wrap<TOut>(),

			_ => throw GetInvalidOperatorException()
		};
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