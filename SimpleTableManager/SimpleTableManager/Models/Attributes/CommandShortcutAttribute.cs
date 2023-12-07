namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandShortcutAttribute : Attribute
{
    public string Key { get; set; }

    public CommandShortcutAttribute(string key)
    {
        Key = key;
    }
}