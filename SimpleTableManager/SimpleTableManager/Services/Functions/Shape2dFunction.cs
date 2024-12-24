// namespace SimpleTableManager.Services.Functions;

// [FunctionMappingType(typeof(Rectangle))]
// [FunctionMappingType(typeof(Ellipse))]
// [FunctionMappingType(typeof(RightTriangle))]
// public class Shape2dFunction : FunctionBase<Shape2dOperator, IShape2d, object>
// {
// 	public override IEnumerable<object> Execute()
// 	{
// 		return Operator switch
// 		{
// 			Shape2dOperator.Const => Arguments.Select(p => p),
// 			Shape2dOperator.Area => Arguments.Select(p => p.Area).Cast<object>(),
// 			Shape2dOperator.Perimeter => Arguments.Select(p => p.Perimeter).Cast<object>(),

// 			_ => throw GetInvalidOperatorException()
// 		};
// 	}
// }
