using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandShortcutAttribute([CallerMemberName] string key = null!) : Attribute
{
	public string Key { get; set; } = key;
}