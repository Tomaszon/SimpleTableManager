using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public abstract class FunctionBase<TOpertor, TIn, TOut> : IFunction
	where TOpertor : struct, Enum
{
	public Dictionary<ArgumentName, IFunctionArgument> NamedArguments { get; set; } = new();

	public IEnumerable<IFunctionArgument> Arguments { get; set; } = Enumerable.Empty<IFunctionArgument>();

	protected IEnumerable<TIn> UnwrappedArguments => Arguments.SelectMany(a => a.Resolve()).Cast<TIn>();

	public TOpertor Operator { get; set; }

	Enum IFunction.Operator { get => Operator; set => Operator = (TOpertor)value; }

	protected string? Error { get; set; }

	public abstract IEnumerable<TOut> ExecuteCore();

	IEnumerable<object> IFunction.Execute()
	{
		return ExecuteWrapper().Cast<object>();
	}

	protected List<TOut> ExecuteWrapper()
	{
		if (Error is null)
		{
			try
			{
				return ExecuteCore().ToList();
			}
			catch (InvalidCastException)
			{
				SetError("Invalid cast");

				throw;
			}
			catch (NullReferenceException)
			{
				SetError("Null reference");

				throw;
			}
			catch (InvalidOperationException)
			{
				SetError("Invalid position");

				throw;
			}
		}
		else
		{
			throw new OperationCanceledException(Error);
		}
	}

	public IEnumerable<string> ExecuteAndFormat()
	{
		var format = GetNamedArgument<string>(ArgumentName.Format);

		var formatter = new ContentFormatter(format);

		return ExecuteWrapper().SelectMany(c => string.Format(formatter, "{0}", c).Split("\r\n", StringSplitOptions.RemoveEmptyEntries));
	}

	public virtual Type GetOutType()
	{
		return typeof(TOut);
	}

	public void SetError(string error)
	{
		Error = error;
	}

	public void ClearError()
	{
		Error = null;
	}

	public string GetError()
	{
		return Error ?? "None";
	}

	public Type GetInType()
	{
		return typeof(TIn);
	}

	public T? GetNamedArgument<T>(ArgumentName key)
		where T : IParsable<T>
	{
		if (NamedArguments.TryGetValue(key, out var argument))
		{
			var result = argument.Resolve().Single();

			return result is string s ? T.Parse(s, null) : (T)result;
		}

		if (GetType().GetCustomAttributes<NamedArgumentAttribute<T>>().SingleOrDefault(p => p.Key == key) is var attribute && attribute is { })
		{
			return attribute.Value;
		}

		return default;
	}

	public Exception GetInvalidOperatorException()
	{
		return new InvalidOperationException($"Operator '{Operator}' is not supported for function type '{GetType().Name}'");
	}
}