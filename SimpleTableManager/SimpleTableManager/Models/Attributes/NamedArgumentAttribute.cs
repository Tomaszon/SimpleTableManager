namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class NamedArgumentAttribute : Attribute
{
	public ArgumentName Key { get; set; }

	public object Value { get; set; }

	public NamedArgumentAttribute(ArgumentName key, object value)
	{
		Key = key;
		Value = value;
	}
}