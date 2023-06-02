namespace SimpleTableManager.Services.Functions
{
	[NamedArgument(ArgumentName.Separator, " ")]
	[NamedArgument(ArgumentName.Trim, ' ')]
	public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
	{
		protected override IEnumerable<object> Execute()
		{
			var separator = GetArgument<string>(ArgumentName.Separator);
			var trim = GetArgument<char>(ArgumentName.Trim);

			return Operator switch
			{
				StringFunctionOperator.Const => Arguments.Cast<object>(),

				StringFunctionOperator.Con => ConcatArguments().Wrap(),

				StringFunctionOperator.Join => JoinArguments(separator).Wrap(),

				StringFunctionOperator.Len => ConcatArguments().Length.Wrap<object>(),

				StringFunctionOperator.Split => Arguments.SelectMany(p => p.Split(separator)),

				StringFunctionOperator.Trim => Arguments.Select(p => p.Trim(trim)),

				StringFunctionOperator.Blow => Arguments.SelectMany(p => p.ToArray()).Cast<object>(),

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