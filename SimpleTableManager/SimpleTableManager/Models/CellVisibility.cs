namespace SimpleTableManager.Models;

public class CellVisibility
{
	public bool IsRowHidden { get; set; }

	public bool IsColumnHidden { get; set; }

	[JsonIgnore]
	public bool IsHidden => IsRowHidden || IsColumnHidden;

	[JsonIgnore]
	public bool IsVisible => !IsHidden;
}