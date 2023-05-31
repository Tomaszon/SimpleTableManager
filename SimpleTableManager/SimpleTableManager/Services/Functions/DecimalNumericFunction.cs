using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models;
using SimpleTableManager.Models.Attributes;

namespace SimpleTableManager.Services.Functions
{
	[NamedArgument(ArgumentName.Decimals, 2)]
	public class DecimalNumericFunction : NumericFunction<decimal, object>
	{
		protected override IEnumerable<object> Execute()
		{
			var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

			return Operator switch
			{
				NumericFunctionOperator.Floor =>
					Arguments.Select(p => (int)decimal.Floor(p)).Cast<object>(),
				NumericFunctionOperator.Ceiling =>
					Arguments.Select(p => (int)decimal.Ceiling(p)).Cast<object>(),
				NumericFunctionOperator.Round =>
					Arguments.Select(p => decimal.Round(p, decimals)).Cast<object>(),

				_ => base.Execute()
			};
		}
	}
}