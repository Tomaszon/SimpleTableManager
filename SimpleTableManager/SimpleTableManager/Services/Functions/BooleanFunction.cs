using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions
{
	public class BooleanFunction : FunctionBase<BooleanFunctionOperator, bool, bool>
	{
		protected override IEnumerable<bool> Execute()
		{
			return Operator switch
			{
				BooleanFunctionOperator.Const => Arguments.Union(ReferenceArguments),
				BooleanFunctionOperator.Not => Arguments.Union(ReferenceArguments).Select(a => !a),
				BooleanFunctionOperator.And => Arguments.Union(ReferenceArguments).All(a => a).Wrap(),
				BooleanFunctionOperator.Or => Arguments.Union(ReferenceArguments).Any(a => a).Wrap(),

				_ => throw GetInvalidOperatorException()
			};
		}
	}
}