namespace SimpleTableManager.Services;

public static class Shared
{
	public static Dictionary<string, string> FRIENDLY_TYPE_NAMES { get; } = new()
	{
		{ "int", "int32" },
		{ "bool", "boolean" }
	};

	public static string NAMED_ARG_SEPARATOR { get; } = ":=";

	public static (Dictionary<ArgumentName, string>, IEnumerable<TType>) SeparateNamedArguments<TType>(params string[] arguments)
	where TType : IParsable<TType>
	{
		var namedArgs = arguments.Where(a => a.Contains(NAMED_ARG_SEPARATOR) == true);

		var regularArgs = arguments.Where(a => !namedArgs.Contains(a)).Select(e => TType.Parse(e, null));

		var namedArgsDic = namedArgs.ToDictionary(k => Enum.Parse<ArgumentName>(k.Split(NAMED_ARG_SEPARATOR)[0], true), v => v.Split(NAMED_ARG_SEPARATOR)[1]);

		return (namedArgsDic, regularArgs);
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
		return type.IsGenericType ? $"{type.GetFriendlyName()}({string.Join(',', type.GenericTypeArguments.Select(t => FormatTypeName(t)))})" : type.GetFriendlyName();
	}

	public static bool IsHandled(this Exception ex)
	{
		return ex is FormatException ||
			ex is InvalidOperationException ||
			ex is TargetParameterCountException ||
			ex is ArgumentException ||
			ex is ArgumentCountException;
	}

	public static void SerializeObject(StreamWriter sw, object? source, TypeNameHandling typeNameHandling = TypeNameHandling.All)
	{
		var serializer = new JsonSerializer
		{
			TypeNameHandling = typeNameHandling,
		};

		serializer.Serialize(new JsonTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented, IndentChar = '\t', }, source);
	}

	public static void DeserializeObject(StreamReader sr, object target)
	{
		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new ClearPropertyContractResolver(),
		};

		serializer.Populate(new JsonTextReader(sr), target);
	}

	public static string SerializeObject(object? source)
	{
		using var m = new MemoryStream();
		using var sw = new StreamWriter(m);
		using var sr = new StreamReader(m);

		SerializeObject(sw, source);

		sw.Flush();

		m.Position = 0;

		return sr.ReadToEnd();
	}

	public static object? DeserializeObject(string state)
	{
		using var m = new MemoryStream();
		using var sw = new StreamWriter(m);
		using var sr = new StreamReader(m);

		sw.Write(state);

		sw.Flush();

		m.Position = 0;

		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new ClearPropertyContractResolver(),
		};

		return serializer.Deserialize(new JsonTextReader(sr));
	}

	
	public static string GetWorkFilePath(string fileName, string extension)
	{
		return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.{extension}");
	}
}