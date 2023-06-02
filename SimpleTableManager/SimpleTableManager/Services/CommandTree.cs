using System.Dynamic;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services
{
	public static class CommandTree
	{
		public static IDictionary<string, object> Commands { get; } = new ExpandoObject();

		public static void FromJsonFolder(string folderPath)
		{
			foreach (var f in Directory.GetFiles(folderPath))
			{
				Commands.Add(Path.GetFileNameWithoutExtension(f), JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(f)));
			}
		}

		public static CommandReference GetCommandReference(string rawCommand, out List<string> arguments)
		{
			var keys = rawCommand.Split(' ', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries).Select(k => k.Replace("\\s", " ").Replace("\\t", " ")).ToList();

			if (keys.Count == 0)
			{
				throw new IncompleteCommandException(rawCommand, Commands.Keys.ToList());
			}

			var methodName = GetReferenceMethodNameRecursive(Commands, keys.First(), keys, rawCommand, out arguments);

			return new CommandReference(keys.FirstOrDefault(), methodName);
		}

		private static string GetReferenceMethodNameRecursive(object obj, string className, List<string> keys, string rawCommand, out List<string> arguments)
		{
			if (obj is ExpandoObject o)
			{
				if (keys.FirstOrDefault() == Shared.HELP_COMMAND)
				{
					throw new HelpRequestedException(rawCommand, o.Select(e => e.Key).ToList(), null);
				}
				else
				{
					var key = keys.FirstOrDefault();

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
							throw new KeyNotFoundException($"Unknow command key '{key}' in '{rawCommand}'");
						}
					}

					if (matchingValue is ExpandoObject && keys.Count <= 1 || !(matchingValue is ExpandoObject) && keys.Count < 1)
					{
						throw new IncompleteCommandException(rawCommand, (matchingValue as ExpandoObject)?.Select(e => e.Key).ToList());
					}

					return GetReferenceMethodNameRecursive(matchingValue, className, keys.GetRange(1, keys.Count - 1), rawCommand, out arguments);
				}
			}
			else
			{
				if (keys.FirstOrDefault() == Shared.HELP_COMMAND)
				{
					throw new HelpRequestedException(rawCommand, null, new CommandReference(className, obj.ToString()));
				}
				else
				{
					arguments = keys;

					return (string)obj;
				}
			}
		}
	}
}
