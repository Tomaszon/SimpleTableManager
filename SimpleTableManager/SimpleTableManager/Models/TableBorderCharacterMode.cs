using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleTableManager.Models
{
	[JsonConverter(typeof(StringEnumConverter)), Flags]
	public enum TableBorderCharacterMode
	{
		None = 0,
		Up = 1,
		UpDouble = 2,
		Left = 4,
		LeftDouble = 8,
		Right = 16,
		RightDouble = 32,
		Down = 64,
		DownDouble = 128,
		Vertical = Up | Down,
		VerticalDouble = UpDouble | DownDouble,
		Horizontal = Left | Right,
		HorizontalDouble = LeftDouble | RightDouble
	}
}
