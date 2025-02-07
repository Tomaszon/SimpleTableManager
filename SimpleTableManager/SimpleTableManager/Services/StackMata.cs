using System.Data;
using System.Text;

namespace SimpleTableManager.Services;

public static class StackMata
{
	private const char _MERGING_OPEN_CHAR = '(';

	private const char _MERGING_CLOSE_CHAR = ')';

	private const char _GROUPING_OPEN_CHAR = '{';

	private const char _GROUPING_CLOSE_CHAR = '}';

	private const char _ESCAPING_CHAR = '\\';

	public static List<List<string>> ProcessArguments(string arguments)
	{
		var mergingDepth = 0;
		var groupingDepth = 0;
		var results = new List<List<string>>();
		var sb = new StringBuilder();
		var escaping = false;

		foreach (char c in arguments)
		{
			switch (c)
			{
				case ' ':
					{
						YieldResult(mergingDepth, groupingDepth, sb, results, c);
					}
					break;

				case _ESCAPING_CHAR:
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

				case _MERGING_OPEN_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							mergingDepth++;
						}
					}
					break;

				case _MERGING_CLOSE_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							if (mergingDepth == 0)
							{
								throw new ArgumentException("Invalid argument merging syntax");
							}

							mergingDepth--;

							if (mergingDepth == 1)
							{
								YieldResult(mergingDepth, groupingDepth, sb, results, c);
							}
						}
					}
					break;

				case _GROUPING_OPEN_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							if (mergingDepth > 0 || groupingDepth > 0)
							{
								throw new ArgumentException("Invalid argument grouping syntax");
							}

							results.Add([]);

							groupingDepth++;
						}
					}
					break;

				case _GROUPING_CLOSE_CHAR:
					{
						if (escaping)
						{
							escaping = false;

							goto default;
						}
						else
						{
							if (groupingDepth == 0)
							{
								throw new ArgumentException("Invalid argument grouping syntax");
							}

							if (groupingDepth == 1)
							{
								YieldResult(mergingDepth, groupingDepth, sb, results, c);
							}

							groupingDepth--;
						}
					}
					break;

				default:
					{
						if (results.Count == 0)
						{
							results.Add([]);
						}

						if (escaping)
						{
							throw new ArgumentException("Invalid argument escaping syntax");
						}

						sb.Append(c);
					}
					break;
			}
		}

		YieldResultCore(groupingDepth, results, sb);

		if (mergingDepth != 0)
		{
			throw new ArgumentException("Invalid argument merging syntax");
		}

		if (escaping)
		{
			throw new ArgumentException("Invalid argument escaping syntax");
		}

		if (groupingDepth != 0)
		{
			throw new ArgumentException("Invalid argument grouping syntax");
		}

		return results;
	}

	private static void YieldResult(int mergingDepth, int groupingDepth, StringBuilder sb, List<List<string>> results, char c)
	{
		if (mergingDepth == 0)
		{
			YieldResultCore(groupingDepth, results, sb);
		}
		else
		{
			sb.Append(c);
		}
	}

	private static void YieldResultCore(int groupingDepth, List<List<string>> results, StringBuilder sb)
	{
		if (sb.Length > 0)
		{
			results.Last().Add(sb.ToString());

			sb.Clear();
		}
	}
}