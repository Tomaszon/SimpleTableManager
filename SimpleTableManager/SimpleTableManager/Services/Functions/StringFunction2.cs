using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class StringFunction2 : Function2<StringFunctionOperator, string>
	{
		public override IEnumerable<object> Execute()
		{
			var separator = NamedArguments["separator"] as string;

			var result = Operator switch
			{
				StringFunctionOperator.Const => Arguments.Cast<object>(),
				StringFunctionOperator.Con => new[] { ConcatArguments() },
				StringFunctionOperator.Join => new[] { JoinArguments(separator) },
				StringFunctionOperator.Len => new object[] { ConcatArguments().Length },
				StringFunctionOperator.Split =>
					Arguments.SelectMany(p => ((string)p).Split(separator))
					.Union(ReferenceArguments.SelectMany(p => ((string)p).Split(separator))),

				_ => throw new System.InvalidOperationException()
			};

			return result;
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