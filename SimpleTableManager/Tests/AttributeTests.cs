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
}