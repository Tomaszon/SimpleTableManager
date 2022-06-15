using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class NumericFunction : Function<decimal>
{
	public NumericFunctionType Type { get; set; }

	public NumericFunction(NumericFunctionType type, params decimal[] arguments) : base(arguments)
	{
		Type = type;
	}

	public override decimal Execute()
	{
		return Type switch
		{
			NumericFunctionType.Sum => Sum().Value,
			NumericFunctionType.Avg => Avg().Value,

			_ => throw new InvalidOperationException()
		};
	}

	private FunctionParameter<decimal> Sum()
	{
		var def = FunctionParameter<decimal>.Default;

		var result = Arguments.Aggregate(def, (a, v) => a += v);

		return result;
	}

	private FunctionParameter<decimal> Avg()
	{
		var result = Sum() / new FunctionParameter<decimal>(Arguments.Count);

		return result;
	}
}

public abstract class Function<T>
{
	public abstract T Execute();

	public List<FunctionParameter<T>> Arguments { get; init; }

	protected Function(params T[] arguments)
	{
		Arguments = arguments.Select(e => new FunctionParameter<T>(e)).ToList();
	}
}