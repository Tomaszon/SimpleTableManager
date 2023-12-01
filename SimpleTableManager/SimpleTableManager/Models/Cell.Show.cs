using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

public partial class Cell
{
	[CommandReference(StateModifier = false)]
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
			Visibility.IsHidden
		};
	}

	[CommandReference(StateModifier = false)]
	public object ShowContentFunction()
	{
		return ContentFunction is null ? "None" : new
		{
			Type = ContentFunction.GetType(),
			ContentFunction.Operator,
			ContentFunction.NamedArguments,
			ContentFunction.Arguments,
			ReturnType = ContentFunction.GetOutType().GetFriendlyName(),
			Error = ContentFunction.GetError()
		};
	}
}