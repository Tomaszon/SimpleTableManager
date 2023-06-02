namespace SimpleTableManager.Services.Functions
{
	[NamedArgument(ArgumentName.Divider, 2)]
	public class IntegerNumericFunction : NumericFunction<int, int>
	{
		protected override IEnumerable<int> Execute()
		{
			var divider = GetArgument<int>(ArgumentName.Divider);

			return Operator switch
			{
				NumericFunctionOperator.Rem => Arguments.Select(p => DivRem(p, divider)),

				NumericFunctionOperator.And => And(Arguments).Wrap(),

				NumericFunctionOperator.Or => Or(Arguments).Wrap(),

				_ => base.Execute()
			};
		}

		private int And(IEnumerable<int> array)
		{
			return array.Aggregate(~0, (a, c) => a &= c);
		}

		private int Or(IEnumerable<int> array)
		{
			return array.Aggregate(0, (a, c) => a |= c);
		}

		private int DivRem(int a, int b)
		{
			System.Math.DivRem(a, b, out var rem);

			return rem;
		}
	}
}