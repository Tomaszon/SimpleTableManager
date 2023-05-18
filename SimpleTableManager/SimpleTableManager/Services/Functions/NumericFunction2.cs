using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace SimpleTableManager.Services.Functions
{
	public abstract class NumericFunction2<TType> : FunctionBase2<NumericFunctionOperator, TType>
		where TType : INumber<TType>, IMinMaxValue<TType>
	{
		public NumericFunction2() : base() { }

		public NumericFunction2(NumericFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<TType> arguments) : base(functionOperator, namedArguments, arguments) { }

		public override IEnumerable<object> Execute(out Type resultType)
		{
			var result = Operator switch
			{
				NumericFunctionOperator.Const => Arguments.ToArray(),
				NumericFunctionOperator.Sum =>
					new[] { Sum(Arguments) + Sum(ReferenceArguments) },
				NumericFunctionOperator.Avg =>
					new[] { (Sum(Arguments) + Sum(ReferenceArguments)) / TType.Parse((Arguments.Count() + ReferenceArguments.Count()).ToString(), NumberStyles.Integer, null) },
				NumericFunctionOperator.Min =>
					new[] { new[] { Min(Arguments), Min(ReferenceArguments) }.Min() },
				NumericFunctionOperator.Max =>
					new[] { new[] { Max(Arguments), Max(ReferenceArguments) }.Max() },

				_ => throw new InvalidOperationException()
			};

			resultType = typeof(TType);

			return result.Cast<object>();
		}

		protected TType Sum(IEnumerable<TType> array)
		{
			return array.Aggregate(TType.AdditiveIdentity, (a, c) => a += c);
		}

		private TType Min(IEnumerable<TType> array)
		{
			return array.Count() > 0 ? array.Min() : TType.MaxValue;
		}

		private TType Max(IEnumerable<TType> array)
		{
			return array.Count() > 0 ? array.Max() : TType.MinValue;
		}
	}
}