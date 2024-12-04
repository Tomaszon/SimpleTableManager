namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class MaxValueAttribute : Attribute
{
	public IComparable Value { get; set; }

	public MaxValueAttribute(int value)
	{
		Value = value;
	}

	public MaxValueAttribute(double value)
	{
		Value = value;
	}

	public MaxValueAttribute(char value)
	{
		Value = value;
	}
}