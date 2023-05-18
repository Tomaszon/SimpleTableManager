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
			var functionOperator = ContentFunction2 is not null ?
				ContentFunction2.GetType().GetProperty(nameof(FunctionBase2<Enum, object>.Operator)).GetValue(ContentFunction2) : null;

			return new
			{
				Size = GetSize().ToString(),
				LayerIndex,
				BorderColor = BorderColor.ToString(),
				Content = new
				{
					Type = ContentType.Name,
					Function = ContentFunction2 is not null ?
						$"{ContentFunction2.GetType().Name}:{functionOperator}" : null,
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
			return ContentFunction2;
		}
	}
}