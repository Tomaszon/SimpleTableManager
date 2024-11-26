namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class NamedArgumentAttribute<T> : Attribute
where T: IParsable<T>
{
	public ArgumentName Key { get; set; }

	public T Value { get; set; }

	public NamedArgumentAttribute(ArgumentName key, T value)
	{
		Key = key;
		Value = value;
	}
}