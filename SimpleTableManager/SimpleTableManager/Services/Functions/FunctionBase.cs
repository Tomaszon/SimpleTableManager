using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Format, "")]
public abstract class FunctionBase<TOpertor, TIn, TOut> : IFunction
	where TOpertor : struct, Enum
{
	public Dictionary<ArgumentName, IFunctionArgument> NamedArguments { get; set; } = new();

	public IEnumerable<IFunctionArgument> Arguments { get; set; } = Enumerable.Empty<IFunctionArgument>();

	protected IEnumerable<TIn> UnwrappedArguments => Arguments.SelectMany(a => a.Resolve()).Cast<TIn>();

	protected IEnumerable<TIn> ConvertedUnwrappedArguments => UnwrappedArgumentsAs(a => ((IConvertible)a).ToType<TIn>());

	//EXPERIMENTAL do not throw null exception in some cases
	protected IEnumerable<TIn> UnwrappedArgumentsAs(Func<object, TIn>? transformation = null)
	{
		transformation ??= a => (TIn)a;

		return Arguments.SelectMany(a => a.Resolve()).Select(transformation);
	}

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
			catch (ArgumentException)
			{
				SetError("Multiple values");

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

	public T GetNamedArgument<T>(ArgumentName key)
		where T : IParsable<T>
	{
		if (NamedArguments.TryGetValue(key, out var argument))
		{
			var results = argument.Resolve() ?? throw new NullReferenceException();

			var result = results.Count() == 1 ? results.Single() : throw new ArgumentException("");

			return result is string s ? T.Parse(s, null) : (T)result;
		}

		if (GetType().GetCustomAttributes<NamedArgumentAttribute<T>>().SingleOrDefault(p => p.Key == key) is var attribute && attribute is { })
		{
			return attribute.Value;
		}

		throw new ArgumentNullException($"Missing named argument '{key}'");
	}

	public Exception GetInvalidOperatorException()
	{
		return new InvalidOperationException($"Operator '{Operator}' is not supported for function type '{GetType().Name}'");
	}
}