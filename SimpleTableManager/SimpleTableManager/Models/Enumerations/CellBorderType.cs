namespace SimpleTableManager.Models.Enumerations
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CellBorderType
	{
		CornerCellClosed,
		CornerCellHorizontal,
		CornerCellVertical,
		CornerCellOpen,
		SiderHorizontal,
		SiderOpen,
		HeaderVertical,
		HeaderOpen,
		ContentClosed,
		ContentOpen,
		ContentUpLeft,
		ContentHorizontalUp,
		ContentVerticalLeft
	}
}
