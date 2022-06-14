using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class NumericFunction<T> : Function<T>
{
	public NumericFunctionType Type { get; set; }

	public NumericFunction(NumericFunctionType type, params T[] arguments) : base(arguments)
	{
		Type = type;
	}

	public override T Execute()
	{
		return Type switch
		{
			NumericFunctionType.Sum => Sum().Value,
			NumericFunctionType.Avg => Avg().Value,

			_ => throw new InvalidOperationException()
		};
	}

	private FunctionParameter<T> Sum()
	{
		return Arguments.Aggregate(FunctionParameter<T>.Default, (a, v) => a += v);
	}

	private FunctionParameter<T> Avg()
	{
		return Sum() / new FunctionParameter<T>((T)Convert.ChangeType(Arguments.Count, typeof(T)));
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
