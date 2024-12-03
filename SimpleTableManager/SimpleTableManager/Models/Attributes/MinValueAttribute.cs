namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class MinValueAttribute : Attribute
{
	public object Value { get; set; }

	public MinValueAttribute(object value)
	{
		Value = value;
	}
}