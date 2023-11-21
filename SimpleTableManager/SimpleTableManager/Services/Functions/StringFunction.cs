namespace SimpleTableManager.Services.Functions
{
	[NamedArgument(ArgumentName.Separator, " ")]
	[NamedArgument(ArgumentName.Trim, ' ')]
	public class StringFunction : FunctionBase<StringFunctionOperator, string, object>
	{
		public override IEnumerable<object> Execute()
		{
			var separator = GetNamedArgument<string>(ArgumentName.Separator)!;
			var trim = GetNamedArgument<char>(ArgumentName.Trim);

			return Operator switch
			{
				StringFunctionOperator.Const => Arguments.Cast<object>(),

				StringFunctionOperator.Con => Concat().Wrap(),

				StringFunctionOperator.Join => Join(separator).Wrap(),

				StringFunctionOperator.Len => Concat().Length.Wrap<object>(),

				StringFunctionOperator.Split => Arguments.SelectMany(p => p.Split(separator)),

				StringFunctionOperator.Trim => Arguments.Select(p => p.Trim(trim)),

				StringFunctionOperator.Blow => Arguments.SelectMany(p => p.ToArray()).Cast<object>(),

				_ => throw GetInvalidOperatorException()
			};
		}

		private string Concat()
		{
			return string.Concat(string.Concat(Arguments), string.Concat(ReferenceArguments));
		}

		private string Join(string separator)
		{
			var value = string.Join(separator, string.Join(separator, Arguments), string.Join(separator, ReferenceArguments));

			return value.Substring(0, value.Length - separator.Length);
		}
	}
}