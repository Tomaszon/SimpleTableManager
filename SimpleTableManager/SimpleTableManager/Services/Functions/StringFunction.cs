using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;

namespace SimpleTableManager.Services.Functions
{
	public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
	{
		public override IEnumerable<object> Execute()
		{
			var separator = NamedArguments.TryGetValue("separator", out var s) ? s : " ";

			return Operator switch
			{
				StringFunctionOperator.Const => Arguments.Cast<object>(),
				StringFunctionOperator.Con => ConcatArguments().Wrap(),
				StringFunctionOperator.Join => JoinArguments(separator).Wrap(),
				StringFunctionOperator.Len => ConcatArguments().Length.Wrap<object>(),
				StringFunctionOperator.Split =>
					Arguments.SelectMany(p => p.Split(separator))
						.Union(ReferenceArguments.SelectMany(p => p.Split(separator))),

				_ => throw new System.InvalidOperationException()
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