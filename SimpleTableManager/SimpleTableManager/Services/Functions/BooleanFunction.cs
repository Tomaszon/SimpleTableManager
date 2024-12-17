namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(bool))]
public class BooleanFunction : FunctionBase<BooleanFunctionOperator, bool, bool>
{
	public override IEnumerable<bool> ExecuteCore()
	{
		return Operator switch
		{
			BooleanFunctionOperator.Const => ConvertedUnwrappedArguments,

			BooleanFunctionOperator.Not => ConvertedUnwrappedArguments.Select(a => !a),

			BooleanFunctionOperator.And => ConvertedUnwrappedArguments.All(a => a).Wrap(),

			BooleanFunctionOperator.Or => ConvertedUnwrappedArguments.Any(a => a).Wrap(),

			BooleanFunctionOperator.IsNotNull => Arguments.Any(a => a.Resolve() is not null).Wrap(),
			
			BooleanFunctionOperator.IsNull => Arguments.All(a => a.Resolve() is null).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}
}