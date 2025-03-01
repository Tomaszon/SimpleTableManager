using System.Globalization;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Format, "")]
[NamedArgument<int>(ArgumentName.First, int.MaxValue)]
[NamedArgument<int>(ArgumentName.Last, int.MaxValue)]
public abstract class FunctionBase<TOperator, TIn, TOut> :
	IFunction
	where TOperator : struct, Enum
	where TIn :  IParsable<TIn>, IConvertible, IComparable
{
	public List<IFunctionArgument> Arguments { get; set; } = [];

	public IEnumerable<ReferenceFunctionArgument> ReferenceArguments =>
		Arguments.Where(a => a is ReferenceFunctionArgument).Cast<ReferenceFunctionArgument>();

	public IEnumerable<IFunctionArgument> UnnamedArguments =>
		Arguments.Where(a => !a.IsNamed);

	public Dictionary<ArgumentName, IFunctionArgument> NamedArguments =>
		Arguments.Where(a => a.IsNamed)
			.ToDictionary(k => (ArgumentName)k.Name!, v => v);

	public IEnumerable<ReferenceFunctionArgument> UnnamedReferenceArguments =>
		UnnamedArguments.Where(a => a is ReferenceFunctionArgument).Cast<ReferenceFunctionArgument>();

	public IEnumerable<IConstFunctionArgument> UnnamedConstArguments =>
		UnnamedArguments.Where(a => a is IConstFunctionArgument).Cast<IConstFunctionArgument>();

	public Dictionary<ArgumentName, IConstFunctionArgument> NamedConstArguments =>
		Arguments.Where(a => a is IConstFunctionArgument && a.IsNamed)
			.ToDictionary(k => (ArgumentName)k.Name!, v => (IConstFunctionArgument)v);

	public Dictionary<ArgumentName, ReferenceFunctionArgument> NamedReferenceArguments =>
		Arguments.Where(a => a is ReferenceFunctionArgument && a.IsNamed)
			.ToDictionary(k => (ArgumentName)k.Name!, v => (ReferenceFunctionArgument)v);

	protected IEnumerable<TIn> UnwrapUnnamedArgumentsAs(Func<object, TIn>? transformation = null)
	{
		transformation ??= a => (TIn)a;

		var first = GetNamedArgument<int>(ArgumentName.First);
		var last = GetNamedArgument<int>(ArgumentName.Last);

		return UnnamedArguments.TakeAround(first, last).SelectMany(a => a.Resolve()).Select(transformation);
	}

	protected IEnumerable<TIn> UnwrappedUnnamedArguments =>
		UnwrapUnnamedArgumentsAs(a => ((IConvertible)a).ToType<TIn>());

	public TOperator Operator { get; set; }

	Enum IFunction.Operator { get => Operator; set => Operator = (TOperator)value; }

	protected string? Error { get; set; }

	public abstract IEnumerable<TOut> ExecuteCore();

	public virtual string GetFriendlyName()
	{
		return typeof(TIn).GetFriendlyName();
	}

	IEnumerable<object> IFunction.Execute()
	{
		return ExecuteWrapper().Cast<object>();
	}

	protected List<TOut> ExecuteWrapper()
	{
		if (Error is not null)
		{
			throw new OperationCanceledException(Error);
		}

		try
		{
			return [.. ExecuteCore()];
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
		catch (FormatException)
		{
			SetError("Argument error");

			throw;
		}
	}

	public IEnumerable<string> ExecuteAndFormat()
	{
		if (Error is not null)
		{
			throw new OperationCanceledException(Error);
		}

		try
		{
			var format = GetNamedArgument<string>(ArgumentName.Format);

			var formatter = new ContentFormatter(format);

			return [.. ExecuteWrapper().SelectMany(c => string.Format(formatter, "{0}", c).Split("\n", StringSplitOptions.RemoveEmptyEntries))];
		}
		catch (FormatException)
		{
			SetError("Format error");

			throw;
		}
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

	public bool TryGetNamedArgument<T>(ArgumentName key, [NotNullWhen(true)] out T? value)
		where T : IParsable<T>, IConvertible
	{
		try
		{
			value = GetNamedArgument<T>(key);

			return true;
		}
		catch (ArgumentNullException)
		{
			value = default;

			return false;
		}
	}

	public T GetNamedArgument<T>(ArgumentName key)
		where T : IParsable<T>, IConvertible
	{
		if (NamedArguments.TryGetValue(key, out var argument))
		{
			var results = argument.Resolve() ?? throw new NullReferenceException();

			var result = results.Single();

			return result is string s ? T.Parse(s, CultureInfo.CurrentUICulture) : ((IConvertible)result).ToType<T>();
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
		var args = UnnamedArguments
			.OrderBy(a => a.GroupingId)
			.ThenBy(a => a is IConstFunctionArgument ? 0 : 1)
			.GroupBy(a => a.GroupingId);

		var groupedArgs = args.Select(FormatArgGroup);

		var constNamedArgs = NamedConstArguments
			.Select(p => $"{p.Key}:{p.Value.NamedValue}");

		var refNamedArgs = NamedReferenceArguments
			.Select(p => $"{p.Key}:{p.Value.Reference.ToShortString()}");

		var fnName = GetFriendlyName();

		var jointArgs = string.Join(' ', groupedArgs);

		var jointNamedArgs = Shared.Join(',', constNamedArgs, refNamedArgs);

		return $"{fnName}:{Operator}:({jointArgs})\n{jointNamedArgs}";
	}

	private static string FormatArgGroup(IGrouping<object?, IFunctionArgument> group)
	{
		return $"{(group.Key is not null ? $"{group.Key}:" : "")}{string.Join(',', group.ToList().Select(a =>
			a is ReferenceFunctionArgument ra ?
				ra.Reference.ToShortString() :
				((IConstFunctionArgument)a).Value))}";
	}

	protected FormattableBoolean Greater()
	{
		return CompareCore(e => e == 1);
	}

	protected FormattableBoolean Less()
	{
		return CompareCore(e => e == -1);
	}

	protected FormattableBoolean GreaterOrEquals()
	{
		return CompareCore(e => e >= 0);
	}

	protected FormattableBoolean LessOrEquals()
	{
		return CompareCore(e => e <= 0);
	}

	protected FormattableBoolean Equals()
	{
		return CompareCore(e => e == 0);
	}

	protected FormattableBoolean NotEquals()
	{
		return CompareCore(e => e != 0);
	}

	private bool CompareCore(Func<int, bool> comparer)
	{
		if (TryGetNamedArgument<TIn>(ArgumentName.Reference, out var reference))
		{
			return UnwrappedUnnamedArguments.Select(e =>
				e.CompareTo(reference)).All(comparer);
		}
		else
		{
			if (UnwrappedUnnamedArguments.Count() > 1)
			{
				return UnwrappedUnnamedArguments.Skip(1).Select(e =>
					UnwrappedUnnamedArguments.First().CompareTo(e)).All(comparer);
			}

			return true;
		}
	}
}