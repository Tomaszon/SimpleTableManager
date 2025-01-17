namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(bool))]
public class BooleanFunction : FunctionBase<BooleanFunctionOperator, bool, bool>
{
    public override string GetFriendlyName()
    {
        return typeof(bool).GetFriendlyName();
    }

    public override IEnumerable<bool> ExecuteCore()
	{
		return Operator switch
		{
			BooleanFunctionOperator.Const => UnwrappedUnnamedArguments,

			BooleanFunctionOperator.Not => UnwrappedUnnamedArguments.Select(a => !a),

			BooleanFunctionOperator.And => UnwrappedUnnamedArguments.All(a => a).Wrap(),

			BooleanFunctionOperator.Or => UnwrappedUnnamedArguments.Any(a => a).Wrap(),

			BooleanFunctionOperator.IsNotNull => Arguments.Any(a => a.Resolve() is not null).Wrap(),
			
			BooleanFunctionOperator.IsNull => Arguments.All(a => a.Resolve() is null).Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}
}