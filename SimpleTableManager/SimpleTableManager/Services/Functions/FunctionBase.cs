using System.Globalization;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Format, "")]
[NamedArgument<int>(ArgumentName.First, int.MaxValue)]
[NamedArgument<int>(ArgumentName.Last, int.MaxValue)]
[NamedArgument<bool>(ArgumentName.IgnoreNullReference, false)]
public abstract class FunctionBase<TOperator, TIn, TOut> :
	IFunction
	where TOperator : struct, Enum
	where TIn : IParsable<TIn>, IConvertible, IComparable, TOut
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

	protected IEnumerable<TIn> UnwrappedUnnamedArguments
	{
		get
		{
			var first = GetNamedArgument<int>(ArgumentName.First);
			var last = GetNamedArgument<int>(ArgumentName.Last);
			var ignoreNullReference = GetNamedArgument<bool>(ArgumentName.IgnoreNullReference);


			var args = UnnamedArguments.TakeAround(first, last).SelectMany(a => a.Resolve(ignoreNullReference)).Select(a => ((IConvertible)a).ToType<TIn>());

			args = FilterCore(args, ArgumentName.Greater, GreaterComparer);
			args = FilterCore(args, ArgumentName.Less, LessComparer);
			args = FilterCore(args, ArgumentName.GreaterOrEquals, GreaterOrEqualsComparer);
			args = FilterCore(args, ArgumentName.LessOrEquals, LessOrEqualsComparer);
			args = FilterCore(args, ArgumentName.Equals, EqualsComparer);
			args = FilterCore(args, ArgumentName.NotEquals, NotEqualsComparer);

			return args;
		}
	}

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
			SetError("!CAST");

			throw;
		}
		catch (NullReferenceException)
		{
			SetError("!NULL");

			throw;
		}
		catch (InvalidPositionException)
		{
			SetError("!POSITION");

			throw;
		}
		catch (ArgumentException)
		{
			SetError("!MULTIPLE");

			throw;
		}
		catch (FormatException)
		{
			SetError("!ARGUMENT");

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
			SetError("!FORMAT");

			throw;
		}
	}

	private static Func<int, bool> GreaterComparer => e => e == 1;

	private static Func<int, bool> LessComparer => e => e == -1;

	private static Func<int, bool> GreaterOrEqualsComparer => e => e >= 0;

	private static Func<int, bool> LessOrEqualsComparer => e => e <= 0;

	private static Func<int, bool> EqualsComparer => e => e == 0;

	private static Func<int, bool> NotEqualsComparer => e => e != 0;

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
		if (NamedArguments.TryGetValue(key, out var argument))
		{
			var results = argument.Resolve(false) ?? throw new NullReferenceException();

			var result = results.Single();

			value = result is string s ? T.Parse(s, CultureInfo.CurrentUICulture) : ((IConvertible)result).ToType<T>();

			return true;
		}

		if (GetType().GetCustomAttributes<NamedArgumentAttribute<T>>().SingleOrDefault(p => p.Key == key) is var attribute && attribute is { })
		{
			value = attribute.Value;

			return true;
		}

		value = default;

		return false;
	}

	public T GetNamedArgument<T>(ArgumentName key)
		where T : IParsable<T>, IConvertible
	{
		if (TryGetNamedArgument<T>(key, out var value))
		{
			return value;
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
			.Select(p => $"{p.Key}:{p.Value.Value}");

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

	protected TOut Min(TIn @default)
	{
		return UnwrappedUnnamedArgumentsIfAny(@default, () => UnwrappedUnnamedArguments.Min()!);
	}

	protected TOut Max(TIn @default)
	{
		return UnwrappedUnnamedArgumentsIfAny(@default, () => UnwrappedUnnamedArguments.Max()!);
	}

	protected FormattableBoolean Greater()
	{
		return CompareCore(GreaterComparer);
	}

	protected FormattableBoolean Less()
	{
		return CompareCore(LessComparer);
	}

	protected FormattableBoolean GreaterOrEquals()
	{
		return CompareCore(GreaterOrEqualsComparer);
	}

	protected FormattableBoolean LessOrEquals()
	{
		return CompareCore(LessOrEqualsComparer);
	}

	protected FormattableBoolean Equals()
	{
		return CompareCore(EqualsComparer);
	}

	protected FormattableBoolean NotEquals()
	{
		return CompareCore(NotEqualsComparer);
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


	private IEnumerable<TIn> FilterCore(IEnumerable<TIn> args, ArgumentName argumentName, Func<int, bool> comparer)
	{
		if (TryGetNamedArgument<TIn>(argumentName, out var reference))
		{
			return args.Where(a => comparer(a.CompareTo(reference)));
		}

		return args;
	}

	protected T UnwrappedUnnamedArgumentsIfAny<T>(T @default, Func<T> @else)
	{
		return UnwrappedUnnamedArguments.Any() ? @else.Invoke() : @default;
	}
}