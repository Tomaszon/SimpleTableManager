using System.Security.Cryptography;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public static class Shared
{
	public const string NORMAL_CHAR_CODE = "\x1B[0m";
	public const string BOLD_CHAR_CODE = "\x1B[1m";
	public const string DIM_CHAR_CODE = "\x1B[2m";
	public const string ITALIC_CHAR_CODE = "\x1B[3m";
	public const string UNDERLINED_CHAR_CODE = "\x1B[4m";
	public const string BLINKING_CHAR_CODE = "\x1B[5m";
	public const string STRIKED_CHAR_CODE = "\x1B[9m";
	public const string OVERLINED_CHAR_CODE = "\x1B[53m";
	public const string NAMED_ARG_SEPARATOR = ":=";
	public const char REF_CHAR = '$';

	public static Dictionary<string, string> FRIENDLY_TYPE_NAMES { get; } = new()
	{
		{ "int", $"{nameof(System)}.int32" },
		{ "bool", $"{nameof(System)}.boolean" }
	};

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
			ex is ArgumentCountException ||
			ex is CommandKeyNotFoundException ||
			ex is FileNotFoundException ||
			ex is OperationCanceledException;
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

	public static object? DeserializeObject(string state)
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
			ContractResolver = new ClearPropertyContractResolver(),
		};

		var result = serializer.Deserialize(new JsonTextReader(sr));

		sr.Close();
		sw.Close();
		ms.Close();

		return result;
	}

	public static string GetWorkFilePath(string fileName, string extension)
	{
		return Path.IsPathFullyQualified(fileName) ? fileName : Path.Combine(Settings.Current.DefaultWorkDirectory, $"{fileName}.{extension}");
	}
}