using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

using SimpleTableManager.Extensions;

namespace SimpleTableManager.Services.Functions
{
	public abstract class NumericFunction<TIn> : FunctionBase<NumericFunctionOperator, TIn, object>
		where TIn : INumber<TIn>, IMinMaxValue<TIn>
	{
		public override IEnumerable<object> Execute()
		{
			return Operator switch
			{
				NumericFunctionOperator.Const => Arguments.ToArray().Cast<object>(),
				NumericFunctionOperator.Neg =>
					Arguments.Select(a => -a).Cast<object>(),
				NumericFunctionOperator.Abs =>
					Arguments.Select(a => TIn.Abs(a)).Cast<object>(),
				NumericFunctionOperator.Sum =>
					(Sum(Arguments) + Sum(ReferenceArguments)).Wrap<object>(),
				NumericFunctionOperator.Avg =>
					((Sum(Arguments) + Sum(ReferenceArguments)) / TIn.Parse((Arguments.Count() + ReferenceArguments.Count()).ToString(), NumberStyles.Integer, null)).Wrap<object>(),
				NumericFunctionOperator.Min =>
					new[] { Min(Arguments), Min(ReferenceArguments) }.Min().Wrap<object>(),
				NumericFunctionOperator.Max =>
					new[] { Max(Arguments), Max(ReferenceArguments) }.Max().Wrap<object>(),

				_ => throw new InvalidOperationException()
			};
		}

		protected TIn Sum(IEnumerable<TIn> array)
		{
			return array.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
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