using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class StringFunction2 : FunctionBase2<StringFunctionOperator, string>
	{
		public StringFunction2() : base() { }

		public StringFunction2(StringFunctionOperator functionOperator, Dictionary<string, string> namedArguments, IEnumerable<string> arguments) : base(functionOperator, namedArguments, arguments) { }

		public override IEnumerable<object> Execute(out Type resultType)
		{
			var separator = NamedArguments.TryGetValue("separator", out var s) ? s : " ";

			var result = Operator switch
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

			//TODO handle other cases
			resultType = typeof(string);

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