
// namespace SimpleTableManager.Services.Functions;

// [FunctionMappingType(typeof(Rectangle))]
// [FunctionMappingType(typeof(Ellipse))]
// [FunctionMappingType(typeof(RightTriangle))]
// public class Shape2dFunction : FunctionBase<Shape2dOperator, IShape2d, IConvertible>
// {
// 	public override string GetFriendlyName()
// 	{
// 		return UnwrappedUnnamedArguments.FirstOrDefault()?.GetType().GetFriendlyName() ?? typeof(IShape2d).GetFriendlyName();
// 	}

// 	public override IEnumerable<IConvertible> ExecuteCore()
// 	{
// 		return Operator switch
// 		{
// 			Shape2dOperator.Const => UnwrappedUnnamedArguments.Select(p => p),
// 			Shape2dOperator.Area => UnwrappedUnnamedArguments.Select(p => p.Area).Cast<IConvertible>(),
// 			Shape2dOperator.Perimeter => UnwrappedUnnamedArguments.Select(p => p.Perimeter).Cast<IConvertible>(),

// 			_ => throw GetInvalidOperatorException()
// 		};
// 	}

// 	public override Type GetOutType()
// 	{
// 		return Operator switch
// 		{
// 			Shape2dOperator.Area => typeof(double),
// 			Shape2dOperator.Perimeter => typeof(double),

// 			_ => UnwrappedUnnamedArguments.FirstOrDefault()?.GetType() ?? typeof(IShape2d)
// 		};

// 	}
// }
