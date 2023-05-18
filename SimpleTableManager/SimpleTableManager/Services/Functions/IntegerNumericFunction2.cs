using System.Collections.Generic;

namespace SimpleTableManager.Services.Functions
{
	public class IntegerNumericFunction2 : NumericFunction2<int>
	{
		public IntegerNumericFunction2() : base() { }

		public IntegerNumericFunction2(NumericFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<int> arguments) : base(functionOperator, namedArguments, arguments) { }
	}
}