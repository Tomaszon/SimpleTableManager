using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services.Functions
{
	public class StringFunction2 : Function2<string, object, StringFunctionOperator>
	{
		public override IEnumerable<object> Execute(List<string> referenceArguments = null)
		{
			var separator = GetSeparator();

			IEnumerable<object> result = Operator switch
			{
				StringFunctionOperator.Const => Arguments.ToArray(),
				StringFunctionOperator.Con =>
					new[] { Concat(referenceArguments) },
				StringFunctionOperator.Join =>
					new[] { string.Join(separator, string.Join(separator, Arguments.Skip(1)), string.Join(separator, referenceArguments)) },
				StringFunctionOperator.Len =>
					new object[] { Concat(referenceArguments).Length },
				StringFunctionOperator.Split =>
					Concat(referenceArguments, true).Split(separator),

				_ => throw new System.InvalidOperationException()
			};

			return result;
		}

		private string GetSeparator()
		{
			return Arguments.FirstOrDefault();
		}

		private string Concat(List<string> referenceArguments, bool skipFirstArgument = false)
		{
			return string.Concat(string.Concat(skipFirstArgument ? Arguments.Skip(1) : Arguments), string.Concat(referenceArguments));
		}

		public override string Convert(string value)
		{
			return value;
		}

		public override string ConvertFrom(object value)
		{
			throw new System.NotImplementedException();
		}

		public override object ConvertTo(string value)
		{
			throw new System.NotImplementedException();
		}
	}
}