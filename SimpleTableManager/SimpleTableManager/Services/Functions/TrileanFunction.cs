using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(Trilean))]
public class TrileanFunction : FunctionBase<LogicalFunctionOperator, Trilean, Trilean>
{
	public override IEnumerable<Trilean> ExecuteCore()
	{
		return Operator switch
		{
			LogicalFunctionOperator.Const => UnwrappedUnnamedArguments,
			LogicalFunctionOperator.Not => UnwrappedUnnamedArguments.Select(a => !a),
			LogicalFunctionOperator.And => And().Wrap(),
			LogicalFunctionOperator.Or => Or().Wrap(),
			// LogicalFunctionOperator.IsNotNull => IsNotNull().Wrap(),
			// LogicalFunctionOperator.IsNull => IsNull().Wrap(),
			LogicalFunctionOperator.Min => Min(false).Wrap(),
			LogicalFunctionOperator.Max => Max(true).Wrap(),
			// LogicalFunctionOperator.Greater => Greater().Wrap(),
			// LogicalFunctionOperator.Less => Less().Wrap(),
			// LogicalFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap(),
			// LogicalFunctionOperator.LessOrEquals => LessOrEquals().Wrap(),
			// LogicalFunctionOperator.Equals => Equals().Wrap(),
			// LogicalFunctionOperator.NotEquals => NotEquals().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private Trilean And()
	{
		return UnwrappedUnnamedArguments.Min() ?? 0;
	}

	private Trilean Or()
	{
		return UnwrappedUnnamedArguments.Max() ?? 0;
	}

	// private FormattableBoolean IsNull()
	// {
	// 	return Arguments.All(a =>
	// 	{
	// 		try
	// 		{
	// 			_ = a.Resolve(false).GetEnumerator().MoveNext();

	// 			return false;

	// 		}
	// 		catch (NullReferenceException)
	// 		{
	// 			return true;
	// 		}
	// 	});
	// }

	// private FormattableBoolean IsNotNull()
	// {
	// 	return Arguments.Any(a =>
	// 	{
	// 		try
	// 		{
	// 			_ = a.Resolve(false).GetEnumerator().MoveNext();

	// 			return true;
	// 		}
	// 		catch (NullReferenceException)
	// 		{
	// 			return false;
	// 		}
	// 	});
	// }
}