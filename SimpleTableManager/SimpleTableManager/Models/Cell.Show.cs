namespace SimpleTableManager.Models;

public partial class Cell
{
	[CommandReference]
	public object ShowDetails()
	{
		return new
		{
			Size = GetSize().ToString(),
			LayerIndex,
			BorderColor = BorderColor.ToString(),
			Content = new
			{
				Type = ContentType?.Name ?? "None",
				Function = ContentFunction is not null ?
					$"{ContentFunction.GetType().Name}:{ContentFunction?.Operator}" : null,
				Error = ContentFunction is not null ? ContentFunction.GetError() : "None",
				Value = GetContents(),
				Padding = ContentPadding.ToString(),
				Alignment = ContentAlignment.ToString(),
				Color = ContentColor.ToString(),
			},
			Visibility.IsHidden
		};
	}

	[CommandReference]
	public object ShowContentFunction()
	{
		
		return ContentFunction is null ? "None" : new
		{
			Type = ContentFunction.GetType().Name,
			ContentFunction.Operator,
			ContentFunction.NamedArguments,
			ContentFunction.Arguments,
			ReturnType = ContentFunction.GetReturnType()?.Name ?? "None",
			Error = ContentFunction.GetError()
		};
	}
}