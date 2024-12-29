namespace SimpleTableManager.Models;

public class CellVisibility
{
	public bool IsRowHidden { get; set; }

	public bool IsColumnHidden { get; set; }

	public bool IsHidden => IsRowHidden || IsColumnHidden;

	public bool IsVisible => !IsHidden;
}