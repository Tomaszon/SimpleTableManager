namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	List<IFunctionArgument> Arguments { get; set; }

	IEnumerable<ReferenceFunctionArgument> ReferenceArguments { get; }

	IEnumerable<IConstFunctionArgument> UnnamedConstArguments { get; }

	IEnumerable<ReferenceFunctionArgument> UnnamedReferenceArguments { get; }

	Dictionary<ArgumentName, IConstFunctionArgument> NamedConstArguments { get; }

	Dictionary<ArgumentName, ReferenceFunctionArgument> NamedReferenceArguments { get; }

	Enum Operator { get; set; }

	IEnumerable<IType> Execute();

	IEnumerable<string> ExecuteAndFormat();

	TParse? GetNamedArgument<TParse>(ArgumentName key)
		where TParse : IParsable<TParse>;

	void SetError(string error);

	void ClearError();

	string GetError();

	string GetFriendlyName();

	Type GetInType();

	Type GetOutType();

	void ShiftReferenceArgumentPositions(Size size)
	{
		UnnamedReferenceArguments.ForEach(a => a.Reference.ShiftReferencedPositions(size));
		NamedReferenceArguments.ForEach(a => a.Value.Reference.ShiftReferencedPositions(size));
	}
}