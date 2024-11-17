using System.Dynamic;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public static class CommandTree
{
	public static IDictionary<string, object?> Commands { get; } = new ExpandoObject();

	public static void FromJsonFolder(string folderPath)
	{
		Commands.Clear();

		foreach (var f in Directory.GetFiles(folderPath))
		{
			var expando = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(f))!;

			expando = LocalizeCommandTree(expando);

			var key = Localizer.Localize(typeof(InstanceMap), null, Path.GetFileNameWithoutExtension(f));

			Commands.Add(key , expando);
		}
	}

	public static ExpandoObject LocalizeCommandTree(ExpandoObject original)
	{
		var clone = new ExpandoObject()!;

		foreach (var kvp in original)
		{
			if (Localizer.TryLocalize(typeof(CommandTree), kvp.Key, out var result))
			{
				clone.TryAdd(result, kvp.Value is ExpandoObject o ? LocalizeCommandTree(o) : kvp.Value);
			}
		}

		return clone;
	}

	public static CommandReference GetCommandReference(string rawCommand, out List<string> arguments)
	{
		var keys = rawCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(k => k.Replace("\\s", " ").Replace("\\t", " ")).ToList();

		if (keys.Count == 0)
		{
			throw new IncompleteCommandException(rawCommand, Commands.Keys.ToList());
		}

		var methodName = GetReferenceMethodNameRecursive(Commands, keys.First(), keys, rawCommand, out arguments);

		return new CommandReference(keys.First(), methodName);
	}

	private static string GetReferenceMethodNameRecursive(object obj, string className, List<string> keys, string rawCommand, out List<string> arguments)
	{
		if (obj is ExpandoObject o)
		{
			if (keys.FirstOrDefault() == SmartConsole.HELP_COMMAND)
			{
				throw new HelpRequestedException(rawCommand, o.Select(e => (e.Key, e.Value is not ExpandoObject)).ToList(), null);
			}
			else
			{
				var key = keys.First();

				var matchingValue = o.FirstOrDefault(e =>
					e.Key.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Contains(key, StringComparer.OrdinalIgnoreCase)).Value;

				var partialMatchingValue = o.FirstOrDefault(e =>
					e.Key.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Any(p => p.StartsWith(key, StringComparison.OrdinalIgnoreCase))).Value;

				if (matchingValue is null)
				{
					if (partialMatchingValue is not null)
					{
						throw new PartialKeyException(rawCommand, key);
					}
					else
					{
						throw new CommandKeyNotFoundException(rawCommand, key);
					}
				}

				if (matchingValue is ExpandoObject && keys.Count <= 1 || matchingValue is not ExpandoObject && keys.Count < 1)
				{
					throw new IncompleteCommandException(rawCommand, (matchingValue as ExpandoObject)?.Select(e => e.Key).ToList());
				}

				return GetReferenceMethodNameRecursive(matchingValue, className, keys.GetRange(1, keys.Count - 1), rawCommand, out arguments);
			}
		}
		else
		{
			if (keys.FirstOrDefault() == SmartConsole.HELP_COMMAND)
			{
				throw new HelpRequestedException(rawCommand, null, new CommandReference(className, obj.ToString()!));
			}
			else
			{
				arguments = keys;

				return (string)obj;
			}
		}
	}
}