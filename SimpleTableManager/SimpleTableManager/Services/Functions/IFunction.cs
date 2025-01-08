namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	Dictionary<ArgumentName, IFunctionArgument> NamedArguments { get; set; }

	IEnumerable<IFunctionArgument> Arguments { get; set; }

	IEnumerable<IConstFunctionArgument> ConstArguments { get; }

	IEnumerable<ReferenceFunctionArgument> ReferenceArguments { get; }

	Dictionary<ArgumentName, IConstFunctionArgument> ConstNamedArguments { get; }

	Dictionary<ArgumentName, ReferenceFunctionArgument> ReferenceNamedArguments { get; }

	Enum Operator { get; set; }

	IEnumerable<object> Execute();

	IEnumerable<string> ExecuteAndFormat();

	Type GetOutType();

	TParse? GetNamedArgument<TParse>(ArgumentName key) where TParse : IParsable<TParse>;

	void SetError(string error);

	void ClearError();

	string GetError();

	string GetFriendlyName();

	Type GetInType();

	void ShiftReferenceArgumentPositions(Size size)
	{
		ReferenceArguments.ForEach(a => a.Reference.ShiftReferencedPosition(size));
		ReferenceNamedArguments.ForEach(a => a.Value.Reference.ShiftReferencedPosition(size));
	}
}