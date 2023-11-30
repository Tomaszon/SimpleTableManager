using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public void ModifyContentFunctionOperator(string @operator)
		{
			ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

			//TODO find a way to list possible values
			ContentFunction = FunctionCollection.GetFunction(ContentFunction.GetInType().Name, @operator, ContentFunction.NamedArguments, ContentFunction.Arguments);
		}

		[CommandReference]
		public void ModifyContentFunctionArguments(params string[] arguments)
		{
			ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");
		
			(var namedArgs, var args) = Shared.SeparateNamedArguments<string>(arguments);

			ContentFunction = FunctionCollection.GetFunction(ContentFunction.GetInType().Name, ContentFunction.Operator.ToString(), namedArgs.Count > 0 ? namedArgs : ContentFunction.NamedArguments, args.Any() ? args.Cast<object>() : ContentFunction.Arguments);
		}
	}
}