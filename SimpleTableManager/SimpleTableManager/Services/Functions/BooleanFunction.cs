namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(bool))]
public class BooleanFunction : FunctionBase<BooleanFunctionOperator, bool, bool>
{
	public override IEnumerable<bool> Execute()
	{
		return Operator switch
		{
			BooleanFunctionOperator.Const => UnwrappedArguments,

			BooleanFunctionOperator.Not => UnwrappedArguments.Select(a => !a),

			BooleanFunctionOperator.And => UnwrappedArguments.All(a => a).Wrap(),

			BooleanFunctionOperator.Or => UnwrappedArguments.Any(a => a).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}
}