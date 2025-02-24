namespace SimpleTableManager;

public static class Shared
{
	public const string NAMED_ARG_SEPARATOR = ":=";
	public const char REF_CHAR = '*';
	public const string REGEX_REF_CHAR = @"\*";

	public static Dictionary<Type, string> FRIENDLY_TYPE_NAMES { get; } = new()
	{
		{ typeof(int), "int" },
		{ typeof(long), "int" },
		{ typeof(bool), "bool" },
		{ typeof(double), "fraction" },
		{ typeof(ConvertibleDateOnly), "date" },
		{ typeof(ConvertibleTimeOnly), "time" },
		{ typeof(ConvertibleTimeSpan), "timeSpan" },
		{ typeof(FormattableBoolean), "bool" }
	};

	public static List<int> IndexArray(int size, int from = 0, int step = 1)
	{
		return [.. Enumerable.Repeat(from, size).Select((value, index) => value + index * step)];
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
			ex is ArgumentCountException ||
			ex is CommandKeyNotFoundException ||
			ex is FileNotFoundException ||
			ex is OperationCanceledException;
	}

	public static void SerializeObject(StreamWriter sw, object? source)
	{
		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new WritablePropertyContractResolver()
		};

		serializer.Serialize(new JsonTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented, IndentChar = '\t' }, source);
	}

	public static void PopulateDocument(StreamReader sr, Document document)
	{
		PopulateObject(sr, document);

		document.Tables.ForEach(t => t.ViewOptions.InvokeViewChangedEvent());
	}

	public static void PopulateObject(StreamReader sr, object target)
	{
		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new ClearPropertyContractResolver()
		};
		serializer.Converters.Add(new AppVersionConverter());
		serializer.Converters.Add(new ConvertibleJsonConverter());

		serializer.Populate(new JsonTextReader(sr), target);
	}

	public static T? SerializeClone<T>(T? source)
	{
		var state = SerializeObject(source);

		return (T?)DeserializeObject(source?.GetType(), state);
	}

	public static string SerializeObject(object? source)
	{
		using var ms = new MemoryStream();
		using var sw = new StreamWriter(ms);
		using var sr = new StreamReader(ms);

		SerializeObject(sw, source);

		sw.Flush();

		ms.Position = 0;

		var result = sr.ReadToEnd();

		sr.Close();
		sw.Close();
		ms.Close();

		return result;
	}

	public static object? DeserializeObject(Type? type, string? state)
	{
		using var ms = new MemoryStream();
		using var sw = new StreamWriter(ms);
		using var sr = new StreamReader(ms);

		sw.Write(state);

		sw.Flush();

		ms.Position = 0;

		var serializer = new JsonSerializer
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new ClearPropertyContractResolver()
		};

		serializer.Converters.Add(new ConvertibleJsonConverter());

		var result = serializer.Deserialize(new JsonTextReader(sr), type);

		sr.Close();
		sw.Close();
		ms.Close();

		return result;
	}

	public static string GetWorkFilePath(string fileName, string extension)
	{
		return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.{extension}");
	}

	public static Version GetAppVersion()
	{
		return Assembly.GetExecutingAssembly().GetName().Version!;
	}

	public static Dictionary<string, MethodInfo> GetMethods<T>(Type type, Func<KeyValuePair<T, MethodInfo>, string> keySelector)
		where T : Attribute
	{
		return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.Union(type.GetMethods(BindingFlags.Public | BindingFlags.Static))
			.Select(m =>
				new KeyValuePair<T?, MethodInfo>(m.GetCustomAttribute<T>(false), m)).Where(e =>
				e.Key is not null).ToDictionary(k => keySelector(k!), v => v.Value);
	}

	public static string Join<T>(char separator, params IEnumerable<IEnumerable<T>> values)
	{
		return string.Join(separator, values.Where(v => v.Any()).Select(p => string.Join(separator, p)));
	}
}