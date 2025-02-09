namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class GroupingIdAttribute(object? id) : Attribute
{
    public object? Id { get; set; } = id;
}