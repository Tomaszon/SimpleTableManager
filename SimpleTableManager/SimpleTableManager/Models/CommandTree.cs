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

		public static CommandReference GetCommandReference(string value, out List<string> parameters, out List<string> availableKeys)
		{
			var keys = value.Split(' ').ToList();

			return new CommandReference()
			{
				ClassName = keys.FirstOrDefault(),
				MethodName = GetReferenceMethodNameRecursive(Commands, keys, value, out parameters, out availableKeys)
			};
		}

		private static string GetReferenceMethodNameRecursive(object obj, List<string> keys, string fullValue, out List<string> parameters, out List<string> availableKeys)
		{
			if (obj is ExpandoObject o)
			{
				if (keys.FirstOrDefault() == "-help")
				{
					var l = o.Select(e => e.Key).ToList();

					parameters = null;
					availableKeys = l;

					return null;
				}
				else
				{
					var v = o.FirstOrDefault(e => Regex.IsMatch(keys.FirstOrDefault(), $"^{e.Key}$")).Value;

					if (v is null)
					{
						throw new KeyNotFoundException($"Unknow command key '{keys.FirstOrDefault()}' in '{fullValue}'");
					}

					if (v is ExpandoObject && keys.Count <= 1 || !(v is ExpandoObject) && keys.Count < 1)
					{
						throw new IncompleteCommandException(fullValue);
					}

					return GetReferenceMethodNameRecursive(v, keys.GetRange(1, keys.Count - 1), fullValue, out parameters, out availableKeys);
				}
			}
			else
			{
				if (keys.FirstOrDefault() == "-help")
				{
					parameters = null;
					availableKeys = null;

					return (string)obj;
				}
				else
				{
					parameters = keys;
					availableKeys = null;

					return (string)obj;
				}
			}
		}
	}
}
