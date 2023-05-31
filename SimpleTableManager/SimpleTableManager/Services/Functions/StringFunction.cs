using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models;
using SimpleTableManager.Models.Attributes;

namespace SimpleTableManager.Services.Functions
{
	[NamedArgument(ArgumentName.Separator, " ")]
	public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
	{
		protected override IEnumerable<object> Execute()
		{
			var separator = GetNamedArgument<string>(ArgumentName.Separator);

			return Operator switch
			{
				StringFunctionOperator.Const => Arguments.Cast<object>(),
				StringFunctionOperator.Con => ConcatArguments().Wrap(),
				StringFunctionOperator.Join => JoinArguments(separator).Wrap(),
				StringFunctionOperator.Len => ConcatArguments().Length.Wrap<object>(),
				StringFunctionOperator.Split =>
					Arguments.SelectMany(p => p.Split(separator))
						.Union(ReferenceArguments.SelectMany(p => p.Split(separator))),

				_ => throw GetInvalidOperatorException()
			};
		}

		private string ConcatArguments()
		{
			return string.Concat(string.Concat(Arguments), string.Concat(ReferenceArguments));
		}

		private string JoinArguments(string separator)
		{
			var value = string.Join(separator, string.Join(separator, Arguments), string.Join(separator, ReferenceArguments));

			return value.Substring(0, value.Length - separator.Length);
		}
	}
}