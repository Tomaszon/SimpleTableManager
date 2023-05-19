using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class StringFunction : FunctionBase<StringFunctionOperator, string>
	{
		public StringFunction() : base() { }

		public StringFunction(StringFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<string> arguments) : base(functionOperator, namedArguments, arguments) { }

		public override IEnumerable<object> Execute()
		{
			var separator = NamedArguments.TryGetValue("separator", out var s) ? s : " ";

			return Operator switch
			{
				StringFunctionOperator.Const => Arguments.Cast<object>(),
				StringFunctionOperator.Con => new[] { ConcatArguments() },
				StringFunctionOperator.Join => new[] { JoinArguments((string)separator) },
				StringFunctionOperator.Len => new object[] { ConcatArguments().Length },
				StringFunctionOperator.Split =>
					Arguments.SelectMany(p => ((string)p).Split((string)separator))
					.Union(ReferenceArguments.SelectMany(p => ((string)p).Split((string)separator))),

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