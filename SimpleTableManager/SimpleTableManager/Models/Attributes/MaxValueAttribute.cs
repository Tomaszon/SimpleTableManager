namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class MaxValueAttribute : Attribute
{
	public object Value { get; set; }

	public MaxValueAttribute(object value)
	{
		Value = value;
	}
}