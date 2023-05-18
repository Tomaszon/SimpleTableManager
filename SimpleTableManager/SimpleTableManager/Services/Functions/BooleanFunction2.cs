using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class BooleanFunction2 : FunctionBase2<BooleanFunctionOperator, bool>
	{
		public BooleanFunction2(BooleanFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<bool> arguments) : base(functionOperator, namedArguments, arguments) { }

		public override IEnumerable<object> Execute(out Type resultType)
		{
			var result = Operator switch
			{
				BooleanFunctionOperator.Not => Arguments.Union(ReferenceArguments).Select(a => !a).Cast<object>(),
				BooleanFunctionOperator.And => new object[] { Arguments.Union(ReferenceArguments).All(a => a) },
				BooleanFunctionOperator.Or => new object[] { Arguments.Union(ReferenceArguments).Any(a => a) },

				_ => throw new System.InvalidOperationException()
			};

			resultType = typeof(bool);

			return result;
		}
	}
}