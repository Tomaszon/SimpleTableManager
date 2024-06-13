using System.Numerics;
using System.Security.Cryptography;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class AreaFunction : FunctionBase<AreaFunctionOperator, Shape, decimal>
{
	public override IEnumerable<decimal> Execute()
	{
		//TODO produce multiple results for shapes with multiple different sides?
		return Operator switch
		{
			AreaFunctionOperator.Rectangle => Arguments.Select(p => p.Size1 * (p.Size2 ?? p.Size1)),

			AreaFunctionOperator.Ellipse =>
				Arguments.Select(p => p.Size1 * (p.Size2 ?? p.Size1) * Math.PI.ToType<decimal>()),

			AreaFunctionOperator.RightTriangle => Arguments.Select(p => p.Size1 * (p.Size2 ?? p.Size1) / 2),

			AreaFunctionOperator.IsoscaleTriangle => Arguments.Select(p => (p.Size2 ?? p.Size1) / 4 * Math.Sqrt((4 * p.Size1 * p.Size1 - ((p.Size2 * p.Size2) ?? (p.Size1 * p.Size1))).ToType<double>()).ToType<decimal>()),

			_ => throw GetInvalidOperatorException()
		};
	}
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class FunctionMappingTypeAttribute : Attribute
{
	public Type MappingType { get; }

	public FunctionMappingTypeAttribute(Type mappingType)
	{
		MappingType = mappingType;
	}
}

[FunctionMappingType(typeof(Rectangle))]
[FunctionMappingType(typeof(Ellipse))]
[FunctionMappingType(typeof(RightTriangle))]
public class Shape2dFunction : FunctionBase<Shape2dOperator, IShape2d, decimal>
{
	public override IEnumerable<decimal> Execute()
	{
		return Operator switch
		{
			Shape2dOperator.Area => Arguments.Select(p => p.Area),
			Shape2dOperator.Perimeter => Arguments.Select(p => p.Perimeter),

			_ => throw GetInvalidOperatorException()
		};
	}
}
