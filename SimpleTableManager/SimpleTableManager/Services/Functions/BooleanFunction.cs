namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(bool))]
public class BooleanFunction : FunctionBase<BooleanFunctionOperator, bool, bool>
{
	public override IEnumerable<bool> Execute()
	{
		return Operator switch
		{
			BooleanFunctionOperator.Const => Arguments,//.Union(ReferenceArguments),

			BooleanFunctionOperator.Not => Arguments/*.Union(ReferenceArguments)*/.Select(a => !a),

			BooleanFunctionOperator.And => Arguments/*.Union(ReferenceArguments)*/.All(a => a).Wrap(),

			BooleanFunctionOperator.Or => Arguments/*.Union(ReferenceArguments)*/.Any(a => a).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}
}