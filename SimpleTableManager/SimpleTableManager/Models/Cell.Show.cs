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
			var funcDic = ContentFunction is not null ?
				ContentFunction.GetType().GetProperties().ToDictionary(k => k.Name, v => v.GetValue(ContentFunction)) : null;

			return new
			{
				Size = GetSize().ToString(),
				LayerIndex,
				BorderColor = BorderColor.ToString(),
				Content = new
				{
					Type = ContentType.Name,
					Function = funcDic is not null ?
						$"{funcDic[nameof(Function<Enum, object>.TypeName)]}:{funcDic[nameof(Function<Enum, object>.Operator)]}" : null,
					Padding = ContentPadding.ToString(),
					Alignment = ContentAlignment.ToString(),
					Color = ContentColor.ToString(),
				}
			};
		}

		[CommandReference]
		public object ShowContentFunction()
		{
			return ContentFunction;
		}
	}
}