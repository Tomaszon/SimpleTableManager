using System.Globalization;
using System.Numerics;

namespace SimpleTableManager.Services.Functions
{
	public abstract class NumericFunction<TIn, TOut> : FunctionBase<NumericFunctionOperator, TIn, TOut>
		where TIn : INumber<TIn>, IMinMaxValue<TIn>, TOut
	{
		protected override IEnumerable<TOut> Execute()
		{
			return Operator switch
			{
				NumericFunctionOperator.Const => Arguments.ToArray().Cast<TOut>(),

				NumericFunctionOperator.Neg => Arguments.Select(a => -a).Cast<TOut>(),

				NumericFunctionOperator.Abs => Arguments.Select(a => TIn.Abs(a)).Cast<TOut>(),

				NumericFunctionOperator.Sum => Sum(Arguments.Union(ReferenceArguments)).Wrap<TOut>(),

				NumericFunctionOperator.Sub => Sub(Arguments).Wrap<TOut>(),

				NumericFunctionOperator.Avg => Avg(Arguments.Union(ReferenceArguments)).Wrap<TOut>(),

				NumericFunctionOperator.Min => new[] { Min(Arguments), Min(ReferenceArguments) }.Min().Wrap<TOut>(),

				NumericFunctionOperator.Max => new[] { Max(Arguments), Max(ReferenceArguments) }.Max().Wrap<TOut>(),

				NumericFunctionOperator.Mul => Multiply(Arguments).Wrap<TOut>(),

				NumericFunctionOperator.Div => Divide(Arguments).Wrap<TOut>(),

				_ => throw GetInvalidOperatorException()
			};
		}

		private TIn Avg(IEnumerable<TIn> array)
		{
			return Sum(array) / TIn.Parse((array.Count()).ToString(), NumberStyles.Integer, null);
		}

		private TIn Multiply(IEnumerable<TIn> array)
		{
			return array.Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= c);
		}

		protected TIn Divide(IEnumerable<TIn> array)
		{
			return array.Skip(1).Aggregate(array.First(), (a, c) => a /= c);
		}

		protected TIn Sum(IEnumerable<TIn> array)
		{
			return array.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
		}

		protected TIn Sub(IEnumerable<TIn> array)
		{
			return array.Skip(1).Aggregate(array.First(), (a, c) => a -= c);
		}

		private TIn Min(IEnumerable<TIn> array)
		{
			return array.Count() > 0 ? array.Min() : TIn.MaxValue;
		}

		private TIn Max(IEnumerable<TIn> array)
		{
			return array.Count() > 0 ? array.Max() : TIn.MinValue;
		}
	}
}