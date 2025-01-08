
namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(Rectangle))]
[FunctionMappingType(typeof(Ellipse))]
[FunctionMappingType(typeof(RightTriangle))]
public class Shape2dFunction : FunctionBase<Shape2dOperator, IShape2d, object>
{
	public override string GetFriendlyName()
	{
		return UnwrappedArguments.FirstOrDefault()?.GetType().GetFriendlyName() ?? typeof(IShape2d).GetFriendlyName();
	}

	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			Shape2dOperator.Const => UnwrappedArguments.Select(p => p),
			Shape2dOperator.Area => UnwrappedArguments.Select(p => p.Area).Cast<object>(),
			Shape2dOperator.Perimeter => UnwrappedArguments.Select(p => p.Perimeter).Cast<object>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			Shape2dOperator.Area => typeof(double),
			Shape2dOperator.Perimeter => typeof(double),

			_ => UnwrappedArguments.FirstOrDefault()?.GetType() ?? typeof(IShape2d)
		};

	}
}
