using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandShortcutAttribute : Attribute
{
    public string Key { get; set; }

    public CommandShortcutAttribute([CallerMemberName] string key = null!)
    {
        Key = key;
    }
}