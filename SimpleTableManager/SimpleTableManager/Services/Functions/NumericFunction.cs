using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace SimpleTableManager.Services.Functions
{
	public abstract class NumericFunction<TType> : FunctionBase<NumericFunctionOperator, TType>
		where TType : INumber<TType>, IMinMaxValue<TType>
	{
		public NumericFunction() : base() { }

		public NumericFunction(NumericFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<TType> arguments) : base(functionOperator, namedArguments, arguments) { }

		public override IEnumerable<object> Execute()
		{
			return Operator switch
			{
				NumericFunctionOperator.Const => Arguments.ToArray().Cast<object>(),
				NumericFunctionOperator.Sum =>
					new object[] { Sum(Arguments) + Sum(ReferenceArguments) },
				NumericFunctionOperator.Avg =>
					new object[] { (Sum(Arguments) + Sum(ReferenceArguments)) / TType.Parse((Arguments.Count() + ReferenceArguments.Count()).ToString(), NumberStyles.Integer, null) },
				NumericFunctionOperator.Min =>
					new object[] { new[] { Min(Arguments), Min(ReferenceArguments) }.Min() },
				NumericFunctionOperator.Max =>
					new object[] { new[] { Max(Arguments), Max(ReferenceArguments) }.Max() },

				_ => throw new InvalidOperationException()
			};
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