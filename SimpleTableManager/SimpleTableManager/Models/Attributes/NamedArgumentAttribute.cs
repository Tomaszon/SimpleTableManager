namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class NamedArgumentAttribute<T>(ArgumentName key, T value) : Attribute
where T : IParsable<T>
{
	public ArgumentName Key { get; set; } = key;

	public T Value { get; set; } = value;
}