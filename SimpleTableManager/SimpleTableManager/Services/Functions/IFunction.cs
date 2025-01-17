namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	IEnumerable<IFunctionArgument> Arguments { get; set; }

	IEnumerable<IConstFunctionArgument> UnnamedConstArguments { get; }

	IEnumerable<ReferenceFunctionArgument> UnnamedReferenceArguments { get; }

	Dictionary<ArgumentName, IConstFunctionArgument> NamedConstArguments { get; }

	Dictionary<ArgumentName, ReferenceFunctionArgument> NamedReferenceArguments { get; }

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
		UnnamedReferenceArguments.ForEach(a => a.Reference.ShiftReferencedPositions(size));
		NamedReferenceArguments.ForEach(a => a.Value.Reference.ShiftReferencedPositions(size));
	}
}