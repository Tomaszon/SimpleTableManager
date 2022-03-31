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
				if (keys.FirstOrDefault() == "-help")
				{
					var l = o.Select(e => e.Key).ToList();

					arguments = null;
					availableKeys = l;

					return null;
				}
				else
				{
					//TODO given command 'hp' maps to configured command 'padding|p'
					var v = o.FirstOrDefault(e => Regex.IsMatch(keys.FirstOrDefault(), $"^{e.Key}$", RegexOptions.IgnoreCase)).Value;

					if (v is null)
					{
						throw new KeyNotFoundException($"Unknow command key '{keys.FirstOrDefault()}' in '{fullValue}'");
					}

					if (v is ExpandoObject && keys.Count <= 1 || !(v is ExpandoObject) && keys.Count < 1)
					{
						throw new IncompleteCommandException(fullValue);
					}

					return GetReferenceMethodNameRecursive(v, keys.GetRange(1, keys.Count - 1), fullValue, out arguments, out availableKeys);
				}
			}
			else
			{
				if (keys.FirstOrDefault() == "-help")
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
