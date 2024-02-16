namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class NamedArgumentAttribute : Attribute
{
	public ArgumentName Key { get; set; }

	public IConvertible Value { get; set; }

	public NamedArgumentAttribute(ArgumentName key, object value)
	{
		Key = key;
		Value = (IConvertible)value;
	}
}