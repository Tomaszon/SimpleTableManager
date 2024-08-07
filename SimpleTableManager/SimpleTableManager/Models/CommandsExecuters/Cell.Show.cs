using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction(StateModifier = false), CommandShortcut("showCellDetails")]
	public object ShowDetails()
	{
		return new
		{
			Size = GetSize().ToString(),
			LayerIndex,
			BorderColor = BorderColor.ToString(),
			Content = new
			{
				Function = ContentFunction is not null ?
					$"{ContentFunction.GetType().Name}:{ContentFunction?.Operator}" : null,
				Padding = ContentPadding.ToString(),
				Alignment = ContentAlignment.ToString(),
				Color = ContentColor.ToString(),
			},
			Comment,
			Visibility.IsHidden,
			ReferencedCell = ReferencedObject is not null ? Table[(Cell)ReferencedObject] : null
		};
	}

	[CommandFunction(IgnoreReferencedObject = true, StateModifier = false)]
	public object ShowSelfDetails()
	{
		return ShowDetails();
	}

	[CommandFunction(StateModifier = false)]
	public object ShowContentFunction()
	{
		return ContentFunction is null ? "None" : new
		{
			Type = ContentFunction.GetType().Name,
			ContentFunction.Operator,
			ContentFunction.NamedArguments,
			ContentFunction.Arguments,
			ReturnType = ContentFunction.GetOutType().GetFriendlyName(),
			Error = ContentFunction.GetError()
		};
	}

	[CommandFunction(IgnoreReferencedObject = true, StateModifier = false)]
	public object ShowSelfContentFunction()
	{
		return ShowContentFunction();
	}
}