using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class DecimalNumericFunction : NumericFunction<decimal>
	{
		public override IEnumerable<object> Execute()
		{
			var decimals = NamedArguments.TryGetValue("decimals", out var v) ? int.Parse(v) : 28;

			return Operator switch
			{
				NumericFunctionOperator.Floor =>
					new[] { Arguments.Select(p => (int)decimal.Floor(p)) },
				NumericFunctionOperator.Ceiling =>
					new[] { Arguments.Select(p => (int)decimal.Ceiling(p)) },
				NumericFunctionOperator.Round =>
					new[] { Arguments.Select(p => decimal.Round(p, decimals)) },
				NumericFunctionOperator.Avg =>
					new object[] { decimal.Round((Sum(Arguments) + Sum(ReferenceArguments)) / (Arguments.Count() + ReferenceArguments.Count()), decimals) },

				_ => base.Execute()
			};
		}
	}
}