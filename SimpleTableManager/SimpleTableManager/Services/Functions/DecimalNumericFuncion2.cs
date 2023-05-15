using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SimpleTableManager.Services.Functions
{
	public class DecimalNumericFuncion2 : NumericFunction2<decimal, decimal>
	{
		public override IEnumerable<decimal> Execute(List<decimal> referenceArguments = null)
		{
			var decimals = Arguments.FirstOrDefault();

			return Operator switch
			{
				NumericFunctionOperator.Floor =>
					Arguments.Select(p => decimal.Floor(p)),
				NumericFunctionOperator.Ceiling =>
					Arguments.Select(p => decimal.Ceiling(p)),
				NumericFunctionOperator.Round =>
					Arguments.Skip(1).Select(p => decimal.Round(p, (int)decimals)),

				_ => base.Execute(referenceArguments)
			};

		}

		public override decimal Cast<T>(INumber<T> value)
		{
			throw new System.NotImplementedException();
		}

		public override decimal Convert(decimal value)
		{
			return value;
		}

		public override object ConvertTo(decimal value)
		{
			throw new System.NotImplementedException();
		}

		public override decimal ConvertFrom(object value)
		{
			throw new System.NotImplementedException();
		}
	}
}