using System.Dynamic;

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
}