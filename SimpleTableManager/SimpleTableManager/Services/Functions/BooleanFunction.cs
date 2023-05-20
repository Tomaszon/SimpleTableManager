using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class BooleanFunction : FunctionBase<BooleanFunctionOperator, bool>
	{
		public override IEnumerable<object> Execute()
		{
			return Operator switch
			{
				BooleanFunctionOperator.Const => Arguments.Union(ReferenceArguments).Cast<object>(),
				BooleanFunctionOperator.Not => Arguments.Union(ReferenceArguments).Select(a => !a).Cast<object>(),
				BooleanFunctionOperator.And => new object[] { Arguments.Union(ReferenceArguments).All(a => a) },
				BooleanFunctionOperator.Or => new object[] { Arguments.Union(ReferenceArguments).Any(a => a) },

				_ => throw new System.InvalidOperationException()
			};
		}
	}
}