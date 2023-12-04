namespace SimpleTableManager.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum IndexCellType
{
	Header,
	Sider
}
