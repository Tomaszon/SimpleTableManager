using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public static class CommandTree
	{
		private const string _REGEX_PREFIX = "regex:";

		private const string _HELP_COMMAND = "help";

		public static IDictionary<string, object> Commands { get; } = new ExpandoObject();

		public static void FromJsonFolder(string folderPath)
		{
			foreach (var f in Directory.GetFiles(folderPath))
			{
				Commands.Add(Path.GetFileNameWithoutExtension(f), JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(f)));
			}
		}

		public static CommandReference GetCommandReference(string value, out List<string> arguments, out List<string> availableKeys)
		{
			var keys = value.Split(' ', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries).Select(k => k.Replace("\\s", " ").Replace("\\t", " ")).ToList();

			if (keys.Count == 0)
			{
				throw new IncompleteCommandException(null);
			}

			return new CommandReference()
			{
				ClassName = keys.FirstOrDefault(),
				MethodName = GetReferenceMethodNameRecursive(Commands, keys, value, out arguments, out availableKeys)
			};
		}

		private static string GetReferenceMethodNameRecursive(object obj, List<string> keys, string fullValue, out List<string> arguments, out List<string> availableKeys)
		{
			if (obj is ExpandoObject o)
			{
				if (keys.FirstOrDefault() == _HELP_COMMAND)
				{
					var l = o.Select(e => e.Key).ToList();

					arguments = null;
					availableKeys = l;

					return _HELP_COMMAND;
				}
				else
				{
					var value = o.FirstOrDefault(e =>
						e.Key.StartsWith(_REGEX_PREFIX) && Regex.IsMatch(keys.FirstOrDefault(), e.Key.Substring(_REGEX_PREFIX.Length), RegexOptions.IgnoreCase) ||
						!e.Key.StartsWith(_REGEX_PREFIX) && e.Key.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Contains(keys.FirstOrDefault(), StringComparer.OrdinalIgnoreCase)).Value;

					if (value is null)
					{
						throw new KeyNotFoundException($"Unknow command key '{keys.FirstOrDefault()}' in '{fullValue}'");
					}

					if (value is ExpandoObject && keys.Count <= 1 || !(value is ExpandoObject) && keys.Count < 1)
					{
						throw new IncompleteCommandException(fullValue);
					}

					return GetReferenceMethodNameRecursive(value, keys.GetRange(1, keys.Count - 1), fullValue, out arguments, out availableKeys);
				}
			}
			else
			{
				if (keys.FirstOrDefault() == _HELP_COMMAND)
				{
					arguments = null;
					availableKeys = null;

					return (string)obj;
				}
				else
				{
					arguments = keys;
					availableKeys = null;

					return (string)obj;
				}
			}
		}
	}
}
