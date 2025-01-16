namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ConstArgumentValueTypeAttribute<T>() : ConstArgumentValueTypeAttribute(typeof(T));

public abstract class ConstArgumentValueTypeAttribute(Type type) : Attribute
{
	public Type Type { get; } = type;
}