using System.Globalization;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Format, "")]
public abstract class FunctionBase<TOpertor, TIn, TOut> : IFunction
	where TOpertor : struct, Enum
{
	public Dictionary<ArgumentName, IFunctionArgument> NamedArguments { get; set; } = [];

	public IEnumerable<IFunctionArgument> Arguments { get; set; } = [];

	protected IEnumerable<TIn> UnwrappedArguments => UnwrapArgumentsAs(a => ((IConvertible)a).ToType<TIn>());

	public IEnumerable<ReferenceFunctionArgument> ReferenceArguments => Arguments.Where(a => a is ReferenceFunctionArgument).Cast<ReferenceFunctionArgument>();

	public IEnumerable<IConstFunctionArgument> ConstArguments => Arguments.Where(a => a is IConstFunctionArgument).Cast<IConstFunctionArgument>();

	public Dictionary<ArgumentName, IConstFunctionArgument> ConstNamedArguments => NamedArguments.Where(a => a.Value is IConstFunctionArgument).ToDictionary(k => k.Key, v => (IConstFunctionArgument)v.Value);

	public Dictionary<ArgumentName, ReferenceFunctionArgument> ReferenceNamedArguments => NamedArguments.Where(a => a.Value is ReferenceFunctionArgument).ToDictionary(k => k.Key, v => (ReferenceFunctionArgument)v.Value);

	protected IEnumerable<TIn> UnwrapArgumentsAs(Func<object, TIn>? transformation = null)
	{
		transformation ??= a => (TIn)a;

		return Arguments.SelectMany(a => a.Resolve() is var result && result is not null ? result : throw new NullReferenceException()).Select(transformation);
	}

	public TOpertor Operator { get; set; }

	Enum IFunction.Operator { get => Operator; set => Operator = (TOpertor)value; }

	protected string? Error { get; set; }

	public abstract IEnumerable<TOut> ExecuteCore();

	public abstract string GetFriendlyName();

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

			return result is string s ? T.Parse(s, CultureInfo.CurrentUICulture) : (T)result;
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

	public override string? ToString()
	{
		var constArgs = ConstArguments.Select(a => a.Value);

		var refArgs = ReferenceArguments
			.Select(a => a.Reference.ToShortString());

		var constNamedArgs = ConstNamedArguments.Select(p => $"{p.Key}:{p.Value.Value}");

		var refNamedArgs = ReferenceNamedArguments.Select(p => $"{p.Key}:{p.Value.Reference.ToShortString()}");

		var fnName = GetFriendlyName();

		var returnTypeName = GetOutType().GetFriendlyName();

		var jointRefArgs = string.Join(',', constArgs.Union(refArgs));

		var jointRefNamedArgs = string.Join(',', constNamedArgs.Union(refNamedArgs));

		return $"{fnName}:{Operator}{(fnName != returnTypeName ? $":{returnTypeName}" : "")}:({jointRefArgs})\n{jointRefNamedArgs}";
	}
}