using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public abstract class FunctionBase<TOpertor, TIn, TOut> : IFunction
	where TOpertor : struct, Enum
{
	// public IEnumerable<Cell> ReferencedCells { get; set; } = Enumerable.Empty<Cell>();

	//TODO make it work, but how? reference cell or fixed position? how to decide?
	// public IEnumerable<TIn> ReferenceArguments =>
	//ReferencedCells.SelectMany(c => c.ContentFunction.Execute().Cast<TIn>());
	// Enumerable.Empty<TIn>();

	public Dictionary<ArgumentName, string> NamedArguments { get; set; } = new();

	public IEnumerable<IFunctionArgument> Arguments { get; set; } = Enumerable.Empty<IFunctionArgument>();

	// IEnumerable<IFunctionParameter> IFunction.Arguments
	// {
	// 	get => Arguments;
	// 	set => Arguments = value.Cast<ReferenceFunctionParameter<TIn>>();
	// }

	protected IEnumerable<TIn> UnwrappedArguments => Arguments.SelectMany(a => a.Resolve()).Cast<TIn>();

	public TOpertor Operator { get; set; }

	Enum IFunction.Operator { get => Operator; set => Operator = (TOpertor)value; }

	public abstract IEnumerable<TOut> Execute();

	IEnumerable<object> IFunction.Execute()
	{
		return Execute().Cast<object>();
	}

	public IEnumerable<string> ExecuteAndFormat()
	{
		var format = GetNamedArgument<string>(ArgumentName.Format);

		var formatter = new ContentFormatter(format);

		return Execute().SelectMany(c => string.Format(formatter, "{0}", c).Split("\r\n", StringSplitOptions.RemoveEmptyEntries)).ToList();
	}

	public virtual Type GetOutType()
	{
		return typeof(TOut);
	}

	public string GetError()
	{
		try
		{
			ExecuteAndFormat();

			return "None";
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public Type GetInType()
	{
		return typeof(TIn);
	}

	public TParse? GetNamedArgument<TParse>(ArgumentName key)
		where TParse : IParsable<TParse>
	{
		if (NamedArguments.TryGetValue(key, out var s))
		{
			return TParse.Parse(s, null);
		}

		if (GetType().GetCustomAttributes<NamedArgumentAttribute>().SingleOrDefault(p => p.Key == key) is var a && a is { })
		{
			return a.Value.ToType<TParse>();
		}

		return default;
	}

	public Exception GetInvalidOperatorException()
	{
		return new InvalidOperationException($"Operator '{Operator}' is not supported for function type '{GetType().Name}'");
	}
}