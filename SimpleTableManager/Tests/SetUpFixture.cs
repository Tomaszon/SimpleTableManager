using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Tests;

[SetUpFixture, ExcludeFromCodeCoverage]
public class SetUpFixture
{
	[OneTimeSetUp]
	public void Setup()
	{
		Settings.FromJson(@"Configs/settings.json");
		Localizer.FromJson(@"Configs/Localizations");

		var app = new App(new Document(Settings.Current.DefaultTableSize));

		app.Reconfig();

		InstanceMap.Instance.Add(() => app);
		InstanceMap.Instance.Add(() => app.Document);
		InstanceMap.Instance.Add(() => app.Document.GetActiveTable());
		InstanceMap.Instance.Add(() => app.Document.GetActiveTable().GetPrimarySelectedCells());
	}
}
