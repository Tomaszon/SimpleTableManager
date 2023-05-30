using System;
using System.Linq;

using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public object ShowDetails()
		{
			var functionOperator = ContentFunction is not null ?
				ContentFunction.GetType().GetProperty(nameof(IFunction.Operator)).GetValue(ContentFunction) : null;

			return new
			{
				Size = GetSize().ToString(),
				LayerIndex,
				BorderColor = BorderColor.ToString(),
				Content = new
				{
					Type = ContentType?.Name ?? "None",
					Function = ContentFunction is not null ?
						$"{ContentFunction.GetType().Name}:{functionOperator}" : null,
					Error = ContentFunction is not null ? ContentFunction.GetError() : "None",
					Value = GetContents(),
					Padding = ContentPadding.ToString(),
					Alignment = ContentAlignment.ToString(),
					Color = ContentColor.ToString(),
				},
				IsHidden = Visibility.IsHidden
			};
		}

		[CommandReference]
		public IFunction ShowContentFunction()
		{
			return ContentFunction;
		}
	}
}