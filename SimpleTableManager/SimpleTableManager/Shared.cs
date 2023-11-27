namespace SimpleTableManager.Services;

public static class Shared
{
	public const string NAMED_ARG_SEPARATOR = ":=";

	public static (Dictionary<ArgumentName, string>, IEnumerable<TType>) SeparateNamedArguments<TType>(params string[] arguments)
	where TType : IParsable<TType>
	{
		var namedArgs = arguments.Where(a => a.Contains(NAMED_ARG_SEPARATOR) == true);

		var regularArgs = arguments.Where(a => !namedArgs.Contains(a)).Select(e => TType.Parse(e, null));

		var namedArgsDic = namedArgs.ToDictionary(k => Enum.Parse<ArgumentName>(k.Split(NAMED_ARG_SEPARATOR)[0], true), v => v.Split(NAMED_ARG_SEPARATOR)[1]);

		return (namedArgsDic, regularArgs);
	}

	public static void Validate(Func<bool> validator, string error)
	{
		Validate<InvalidOperationException>(validator, error);
	}

	public static void Validate<T>(Func<bool> validator, string error) where T : Exception
	{
		if (!validator())
		{
			throw (T)Activator.CreateInstance(typeof(T), error)!;
		}
	}

	public static List<int> IndexArray(int size, int from = 0, int step = 1)
	{
		return Enumerable.Repeat(from, size).Select((value, index) => value + index * step).ToList();
	}

	public static int Max(params int[] values)
	{
		return Enumerable.Max(values);
	}

	public static int Min(params int[] values)
	{
		return Enumerable.Min(values);
	}

	public static void StepCursor(int x, int y)
	{
		Console.SetCursorPosition(Console.CursorLeft + x, Console.CursorTop + y);
	}

	public static string FormatTypeName(Type type)
	{
		return type.IsGenericType ? $"{type.Name}({string.Join(',', type.GenericTypeArguments.Select(t => FormatTypeName(t)))})" : type.Name;
	}

	public static bool IsHandled(this Exception ex)
	{
		return ex is FormatException || ex is InvalidOperationException;
	}
}