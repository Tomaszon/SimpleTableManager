using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Models.Enumerations;

namespace SimpleTableManager.Services.Functions
{
	[NamedArgument(ArgumentName.Separator, "")]
	[NamedArgument(ArgumentName.Count, 1)]
	public class CharacterFunction : FunctionBase<CharacterFunctionOperator, char, object>
	{
		protected override IEnumerable<object> Execute()
		{
			var separator = GetArgument<string>(ArgumentName.Separator);
			var count = GetArgument<int>(ArgumentName.Count);

			return Operator switch
			{
				CharacterFunctionOperator.Const => Arguments.Cast<object>(),

				CharacterFunctionOperator.Concat => string.Concat(Arguments).Wrap(),

				CharacterFunctionOperator.Join => string.Join(separator, Arguments).Wrap(),

				CharacterFunctionOperator.Repeat => Arguments.Select(c => new string(c, count)),

				_ => throw GetInvalidOperatorException()
			};
		}
	}
}