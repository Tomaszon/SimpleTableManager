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

			ReferenceNamedArguments = ContentFunction.NamedArguments.Where(a => a.Value is ReferenceFunctionArgument).ToDictionary(k => k.Key, v =>
				new
				{
					Refrence = ((ReferenceFunctionArgument)v.Value).Reference.ToString(),
					ReferencedValues = ((IFunctionArgument)v.Value).TryResolve(out var result, out var error) && result?.Count() == 1 ? result.Single() : $"Error: '{error}'"
				}),

			ConstNamedArguments = ContentFunction.NamedArguments.Where(a => a.Value is IConstFunctionArgument).ToDictionary(k => k.Key, v => ((IConstFunctionArgument)v.Value).Resolve().Single()),

			ReferenceArguments = ContentFunction.Arguments.Where(a => a is ReferenceFunctionArgument).Cast<ReferenceFunctionArgument>().Select(a =>
				new
				{
					Reference = a.Reference.ToString(),
					ReferencedValues = ((IFunctionArgument)a).TryResolve(out var result, out var error) ? result : $"Error: '{error}'".Wrap()
				}),

			ConstArguments = ContentFunction.Arguments.Where(a => a is IConstFunctionArgument).Cast<IConstFunctionArgument>().SelectMany(a => a.Resolve()),

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