namespace SimpleTableManager.Services;

public static class BorderCharacters
{
	private static List<BorderCharacter> _CHARACTERS = [];

	public static void FromJson(string path)
	{
		_CHARACTERS = JsonConvert.DeserializeObject<List<BorderCharacter>>(File.ReadAllText(path))!;
	}

	public static char Get(BorderType type)
	{
		if (_CHARACTERS.SingleOrDefault(c =>
			c.Type == (BorderType)((int)type & ~(int)BorderType.None)) is var res && res is { })
		{
			return Settings.Current.ModernTableBorder ? res.Modern : res.Retro;
		}
		else
		{
			return 'X';
		}
	}

	public static char GetIntersection(BorderType? up = null, BorderType? down = null, BorderType? left = null, BorderType? right = null)
	{
		return Get(GetIntersectionBorderType(up, down, left, right));
	}

	public static BorderType GetIntersectionBorderType(BorderType? up = null, BorderType? down = null, BorderType? left = null, BorderType? right = null)
	{
		BorderType result = BorderType.None;

		if (up is { })
		{
			result |= up.Value.HasFlag(BorderType.DownDouble) ? BorderType.UpDouble : BorderType.Up;
		}

		if (down is { })
		{
			result |= down.Value.HasFlag(BorderType.UpDouble) ? BorderType.DownDouble : BorderType.Down;
		}

		if (left is { })
		{
			result |= left.Value.HasFlag(BorderType.RightDouble) ? BorderType.LeftDouble : BorderType.Left;
		}

		if (right is { })
		{
			result |= right.Value.HasFlag(BorderType.LeftDouble) ? BorderType.RightDouble : BorderType.Right;
		}

		return (BorderType)((int)result & ~(int)BorderType.None);
	}
}