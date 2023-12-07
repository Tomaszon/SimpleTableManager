namespace SimpleTableManager.Models;

public static class CommandShortcuts
{
	private static Dictionary<(ConsoleModifiers, ConsoleKey), string> _SHORTCUTS = new();

	public static void FromJson(string path)
	{
		var tmp = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path))!;

		_SHORTCUTS = tmp.ToDictionary(k =>
		{
			var e = k.Key.Split('+');

			return (Enum.Parse<ConsoleModifiers>(e[0]), Enum.Parse<ConsoleKey>(e[1]));
		}, v => v.Value);
	}

	public static bool TryGetShortcut(string action, [NotNullWhen(true)] out (ConsoleModifiers, ConsoleKey)? shortcut)
	{
		var result = _SHORTCUTS.SingleOrDefault(e => e.Value.ToLower().Equals(action.ToLower()));

		shortcut = result.Key == default ? null : result.Key;

		return shortcut is not null;
	}


	public static bool TryGetAction(ConsoleKeyInfo keyInfo, [NotNullWhen(true)] out string? action)
	{
		var result = _SHORTCUTS.TryGetValue((keyInfo.Modifiers, keyInfo.Key), out action);

		action = action?.ToLower();

		return result;
	}

	public static Dictionary<string, MethodInfo> GetMethods(Type type)
	{
		return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.Union(type.GetMethods(BindingFlags.Public | BindingFlags.Static))
			.Select(m =>
				(attribute: m.GetCustomAttribute<CommandShortcutAttribute>(false), method: m)).Where(e =>
				e.attribute is not null).ToDictionary(k => k.attribute!.Key.ToLower(), v => v.method);
	}
}
