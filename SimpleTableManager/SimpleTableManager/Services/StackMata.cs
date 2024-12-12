using System.Data;
using System.Text;

namespace SimpleTableManager.Services;

public static class StackMata
{
	private const char _OPEN_CHAR = '(';

	private const char _CLOSE_CHAR = ')';

	private const char _ESCAPE_CHAR = '\\';

	public static List<string> ProcessArguments(string arguments)
	{
		var depth = 0;
		var results = new List<string>();
		var sb = new StringBuilder();
		var escaping = false;

		foreach (char c in arguments)
		{
			switch (c)
			{
				case ' ':
					{
						if (depth == 0)
						{
							results.Add(sb.ToString());

							sb.Clear();
						}
						else
						{
							sb.Append(c);
						}
					}
					break;

				case _ESCAPE_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							escaping = true;
						}
					}
					break;

				case _OPEN_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							depth++;
						}
					}
					break;

				case _CLOSE_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							depth--;
						}
					}
					break;

				default:
					{
						if (escaping)
						{
							throw new ArgumentException("Invalid argument escaping syntax");
						}

						sb.Append(c);
					}
					break;
			}
		}

		results.Add(sb.ToString());

		results = results.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

		if (depth != 0)
		{
			throw new ArgumentException("Invalid argument parenthesis syntax");
		}

		if (escaping)
		{
			throw new ArgumentException("Invalid argument escaping syntax");
		}

		return results;
	}
}