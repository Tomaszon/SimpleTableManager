using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SimpleTableManager.Services.Functions
{
	public abstract class NumericFunction2<TIn, TOut> : Function2<TIn, TOut, NumericFunctionOperator>
		where TIn : INumber<TIn>
	{
		public override IEnumerable<TOut> Execute(List<TIn> referenceArguments = null)
		{
			var result = Operator switch
			{
				NumericFunctionOperator.Const => Arguments.ToArray(),
				NumericFunctionOperator.Sum =>
					new[] { Sum(Arguments) + Sum(referenceArguments) },
				NumericFunctionOperator.Avg =>
					new[] { (Sum(Arguments) + Sum(referenceArguments)) / TIn.Parse((Arguments.Count + referenceArguments.Count).ToString(), System.Globalization.NumberStyles.Integer, null) },
				NumericFunctionOperator.Min =>
					new[] { new[] { Arguments.Min(), referenceArguments.Min() }.Min() },
				NumericFunctionOperator.Max =>
					new[] { new[] { Arguments.Max(), referenceArguments.Max() }.Max() },

				_ => throw new InvalidOperationException()
			};

			return result.Select(r => Convert(r));
		}

		private TIn Sum(IEnumerable<TIn> array)
		{
			return array.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
		}

		public abstract TOut Cast<T>(INumber<T> value) where T : INumber<T>;
	}
}