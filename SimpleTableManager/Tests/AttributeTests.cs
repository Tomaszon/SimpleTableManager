using System.Globalization;

using NUnit.Framework.Internal;

using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class AttributeTests : TestBase
{
	[Test]
	public void CommandInformationAttributes()
	{
		var a1 = new CommandInformationAttribute("Asd");
		var a2 = new CommandInformationAttribute<Document>(nameof(Document.Open));
		var a3 = new CommandInformationAttribute<Document>("Asd");

		var cn = CultureInfo.CurrentUICulture.Name.ToLower();
		var i1 = "Asd";
		var i2 = "Opens the given document";
		var i3 = $"Translation not found: info: {cn}.document.asd.info -> {cn}.info";

		CheckResults([a1.Information, a2.Information, a3.Information], [i1, i2, i3]);
	}

	[Test]
	public void Attributes()
	{
		_ = new CommandFunctionAttribute("ASD")
		{
			Clears = GlobalStorageKey.None,
			ClearsCache = true,
			IgnoreReferencedObject = true,
			StateModifier = true,
			WithSelector = false
		};
		_ = new CommandShortcutAttribute("ASD");
		_ = new GroupingIdAttribute(1);
		_ = new MaxValueAttribute(1);
		_ = new MaxValueAttribute(1.1);
		_ = new MaxValueAttribute('a');
		_ = new MinLengthAttribute(1);
		_ = new MinValueAttribute(1);
		_ = new MinValueAttribute(1.1);
		_ = new MinValueAttribute('a');
		_ = new ValueTypesAttribute<int>();
		_ = new ValueTypesAttribute<int, int>();
		_ = new ValueTypesAttribute<int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int, int, int, int, int, int>();
		_ = new ValueTypesAttribute<int, int, int, int, int, int, int, int, int, int, int, int>();
	}

	[Test]
	public void CommandShortcutAttribute()
	{
	}
}