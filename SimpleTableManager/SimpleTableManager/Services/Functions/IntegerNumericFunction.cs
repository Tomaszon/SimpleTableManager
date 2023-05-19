using System.Collections.Generic;

namespace SimpleTableManager.Services.Functions
{
	public class IntegerNumericFunction : NumericFunction<int>
	{
		public IntegerNumericFunction() : base() { }

		public IntegerNumericFunction(NumericFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<int> arguments) : base(functionOperator, namedArguments, arguments) { }
	}
}