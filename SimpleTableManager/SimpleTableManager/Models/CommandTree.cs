using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public static class CommandTree
	{
		public static ExpandoObject Commands { get; set; }

		public static void FromJson(string path)
		{
			Commands = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(path));
		}

		public static string GetCommandReference(string value, out List<string> parameters, out List<string> availableKeys)
		{
			var keys = value.Split(' ').ToList();

			return GetReferenceRecursive(Commands, keys, value, out parameters, out availableKeys);
		}

		private static string GetReferenceRecursive(object obj, List<string> keys, string fullValue, out List<string> parameters, out List<string> availableKeys)
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
					var v = o.FirstOrDefault(e => e.Key == keys.FirstOrDefault()).Value;

					if (v is null)
					{
						throw new KeyNotFoundException($"Unknow command key '{keys.FirstOrDefault()}' in '{fullValue}'");
					}

					if (keys.Count <= 1)
					{
						throw new InvalidOperationException($"Incomplete command '{fullValue}'");
					}

					return GetReferenceRecursive(v, keys.GetRange(1, keys.Count - 1), fullValue, out parameters, out availableKeys);
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
