using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;

namespace SimpleTableManager.Services.Functions
{
	public class DecimalNumericFunction : NumericFunction<decimal>
	{
		public override IEnumerable<object> Execute()
		{
			var decimals = NamedArguments.TryGetValue("decimals", out var v) ? int.Parse(v) : 2;

			return Operator switch
			{
				NumericFunctionOperator.Floor =>
					Arguments.Select(p => (int)decimal.Floor(p)).Cast<object>(),
				NumericFunctionOperator.Ceiling =>
					Arguments.Select(p => (int)decimal.Ceiling(p)).Cast<object>(),
				NumericFunctionOperator.Round =>
					Arguments.Select(p => decimal.Round(p, decimals)).Cast<object>(),
				NumericFunctionOperator.Avg =>
					decimal.Round((Sum(Arguments) + Sum(ReferenceArguments)) / (Arguments.Count() + ReferenceArguments.Count()), decimals).Wrap<object>(),

				_ => base.Execute()
			};
		}
	}
}