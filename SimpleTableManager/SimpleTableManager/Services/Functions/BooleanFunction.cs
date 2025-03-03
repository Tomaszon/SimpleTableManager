using SimpleTableManager.Models.Enumerations.FunctionOperators;

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
			BooleanFunctionOperator.And => And().Wrap(),
			BooleanFunctionOperator.Or => Or().Wrap(),
			BooleanFunctionOperator.IsNotNull => IsNotNull().Wrap(),
			BooleanFunctionOperator.IsNull => IsNull().Wrap(),
			BooleanFunctionOperator.Greater => Greater().Wrap(),
			BooleanFunctionOperator.Less => Less().Wrap(),
			BooleanFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap(),
			BooleanFunctionOperator.LessOrEquals => LessOrEquals().Wrap(),
			BooleanFunctionOperator.Equals => Equals().Wrap(),
			BooleanFunctionOperator.NotEquals => NotEquals().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private FormattableBoolean And()
	{
		return UnwrappedUnnamedArguments.All(a => a);
	}

	private FormattableBoolean Or()
	{
		return UnwrappedUnnamedArguments.Any(a => a);
	}

	private FormattableBoolean IsNull()
	{
		return Arguments.All(a =>
		{
			try
			{
				_ = a.Resolve(false).GetEnumerator().MoveNext();

				return false;

			}
			catch (NullReferenceException)
			{
				return true;
			}
		});
	}

	private FormattableBoolean IsNotNull()
	{
		return Arguments.Any(a =>
		{
			try
			{
				_ = a.Resolve(false).GetEnumerator().MoveNext();

				return true;
			}
			catch (NullReferenceException)
			{
				return false;
			}
		});
	}
}