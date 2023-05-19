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
				ContentFunction.GetType().GetProperty(nameof(FunctionBase<Enum, object>.Operator)).GetValue(ContentFunction) : null;

			return new
			{
				Size = GetSize().ToString(),
				LayerIndex,
				BorderColor = BorderColor.ToString(),
				Content = new
				{
					Type = ContentType.Name,
					Function = ContentFunction is not null ?
						$"{ContentFunction.GetType().Name}:{functionOperator}" : null,
					Value = GetContents(),
					Padding = ContentPadding.ToString(),
					Alignment = ContentAlignment.ToString(),
					Color = ContentColor.ToString(),
				},
				IsHidden = Visibility.IsHidden
			};
		}

		[CommandReference]
		public object ShowContentFunction()
		{
			return ContentFunction;
		}
	}
}