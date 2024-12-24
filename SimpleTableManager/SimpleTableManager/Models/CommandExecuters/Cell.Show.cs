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
			Comments,
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

			ReferenceNamedArguments = ContentFunction.ReferenceNamedArguments.ToDictionary(k => k.Key, v =>
				new
				{
					Refrence = v.Value.Reference.ToString(),
					ReferencedValues = v.Value.TryResolve(out var result, out var error) && result?.Count() == 1 ? result.Single() : $"Error: '{error}'"
				}),

			ConstNamedArguments = ContentFunction.ConstNamedArguments.ToDictionary(k => k.Key, v => v.Value.Value),

			ReferenceArguments = ContentFunction.ReferenceArguments.Select(a =>
				new
				{
					Reference = a.Reference.ToString(),
					ReferencedValues = a.TryResolve(out var result, out var error) ? result : $"Error: '{error}'".Wrap()
				}),

			ConstArguments = ContentFunction.ConstArguments.Select(a => a.Value),

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