namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(FormattableBoolean))]
public class BooleanFunction : FunctionBase<BooleanFunctionOperator, FormattableBoolean, FormattableBoolean>
{
	public override IEnumerable<FormattableBoolean> ExecuteCore()
	{
		return Operator switch
		{
			BooleanFunctionOperator.Const => UnwrappedUnnamedArguments,

			BooleanFunctionOperator.Not => UnwrappedUnnamedArguments.Select(a => !a),

			BooleanFunctionOperator.And => ((FormattableBoolean)UnwrappedUnnamedArguments.All(a => a)).Wrap(),

			BooleanFunctionOperator.Or => ((FormattableBoolean)UnwrappedUnnamedArguments.Any(a => a)).Wrap(),

			BooleanFunctionOperator.IsNotNull => ((FormattableBoolean)Arguments.Any(a =>
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

			BooleanFunctionOperator.IsNull => ((FormattableBoolean)Arguments.All(a =>
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

			BooleanFunctionOperator.Greater => Greater().Wrap(),

			BooleanFunctionOperator.Less => Less().Wrap(),

			BooleanFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap(),

			BooleanFunctionOperator.LessOrEquals => LessOrEquals().Wrap(),

			BooleanFunctionOperator.Equals => Equals().Wrap(),

			BooleanFunctionOperator.NotEquals => NotEquals().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}
}