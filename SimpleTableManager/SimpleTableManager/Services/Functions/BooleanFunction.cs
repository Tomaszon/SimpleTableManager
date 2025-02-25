namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(BooleanType))]
public class BooleanFunction : FunctionBase<BooleanFunctionOperator, BooleanType, BooleanType>
{
	public override IEnumerable<BooleanType> ExecuteCore()
	{
		return Operator switch
		{
            BooleanFunctionOperator.Const => UnwrappedUnnamedArguments,

            BooleanFunctionOperator.Not => UnwrappedUnnamedArguments.Select(a => !a),

            BooleanFunctionOperator.And => ((BooleanType)UnwrappedUnnamedArguments.All(a => a)).Wrap(),

            BooleanFunctionOperator.Or => ((BooleanType)UnwrappedUnnamedArguments.Any(a => a)).Wrap(),

            BooleanFunctionOperator.IsNotNull => ((BooleanType)Arguments.Any(a =>
			{
				try
				{
					_ = a.Resolve().GetEnumerator().MoveNext();

					return true;
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}))
			.Wrap(),

            BooleanFunctionOperator.IsNull => ((BooleanType)Arguments.All(a =>
			{
				try
				{
					_ = a.Resolve().GetEnumerator().MoveNext();

					return false;

				}
				catch (NullReferenceException)
				{
					return true;
				}
			})).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}
}