namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class MinValueAttribute : Attribute
{
	public IComparable Value { get; set; }

	public MinValueAttribute(int value)
	{
		Value = value;
	}

	public MinValueAttribute(double value)
	{
		Value = value;
	}

	public MinValueAttribute(char value)
	{
		Value = value;
	}
}