using NUnit.Framework.Internal;
using SimpleTableManager.Models.Types;

namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class ExtensionTests : TestBase
{
	[Test]
	[TestCase(typeof(DateType), "date")]
	[TestCase(typeof(TimeType), "time")]
	[TestCase(typeof(Models.BooleanType), "bool")]
	[TestCase(typeof(string), "string")]
	public void TypeFriendlyNames(Type type, string name)
	{
		CheckResult(type.GetFriendlyName().ToLower(), name);
	}

	[Test]
	[TestCase(ContentStyle.Blinking, ContentStyle.Bold, true)]
	[TestCase(ContentStyle.All, ContentStyle.Bold, false)]
	public void NotHasFlag(ContentStyle contentStyle, ContentStyle flag, bool notHas)
	{
		CheckResult(contentStyle.NotHasFlag(flag), notHas);
	}

	[Test]
	public void InnerException()
	{
		var ex = new Exception("asd", new ArgumentException("asd", new InvalidOperationException("asd")));

		var innermost = ex.GetInnermostException();

		CheckResult(innermost.GetType(), typeof(InvalidOperationException));
	}

	[Test]
	public void DictionaryReplace()
	{
		var dictionary = new Dictionary<string, string>() { { "0", "0" }, { "1", "1" } };

		dictionary.Replace("0", "2");

		CheckResult(dictionary["0"], "2");
	}

	[Test]
	public void ForEach()
	{
		IEnumerable<string> collection = new List<string>(["1", "2", "3"]);
		var result = new List<string>();

		collection.ForEach(e => result.Add($"{e}++"));

		CheckResults(result, ["1++", "2++", "3++"]);
	}

	[Test]
	[TestCase("asd", "Asd")]
	[TestCase("123", "123")]
	public void StringToUpperFirst(string s1, string s2)
	{
		CheckResult(s1.ToUpperFirst(), s2);
	}

	[Test]
	[TestCase("asd", '+', 3, "+++asd")]
	public void AppendLeft(string value, char appendChar, int count, string result)
	{
		CheckResult(value.AppendLeft(appendChar, count), result);
	}

	[Test]
	[TestCase("asd", '+', 3, "asd+++")]
	public void AppendRight(string value, char appendChar, int count, string result)
	{
		CheckResult(value.AppendRight(appendChar, count), result);
	}

	[Test]
	[TestCase("asd", '+', 3, 1, "+++asd+")]
	public void AppendLeftRight(string value, char appendChar, int leftCount, int rightCount, string result)
	{
		CheckResult(value.AppendLeftRight(appendChar, leftCount, rightCount), result);
	}

	[Test]
	[TestCase("asd", 7, "  asd  ")]
	public void PadLeftRight(string value, int totalWidth, string result)
	{
		CheckResult(value.PadLeftRight(totalWidth), result);
	}


	[Test]
	[TestCase(int.MaxValue, int.MaxValue, new[] { 1, 2, 3, 4, 5 })]
	[TestCase(2, int.MaxValue, new[] { 1, 2 })]
	[TestCase(int.MaxValue, 2, new[] { 4, 5 })]
	[TestCase(2, 2, new[] { 1, 2, 4, 5 })]
	[TestCase(-1,0, new int[] { })]
	public void TakeAround(int first, int last, int[] expected)
	{
		var values = new int[] { 1, 2, 3, 4, 5 };

		var results = values.TakeAround(first, last);

		CheckResults([.. results], expected);
	}
}