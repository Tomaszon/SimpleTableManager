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

	IEnumerable<object> Execute();

	IEnumerable<string> ExecuteAndFormat();

	TParse GetNamedArgument<TParse>(ArgumentName key)
		where TParse : IParsable<TParse>, IConvertible;

	bool TryGetNamedArgument<TParse>(ArgumentName key, out TParse? value)
		where TParse : IParsable<TParse>, IConvertible;

	void SetError(string error);

	void ClearError();

	string GetError();

	string GetFriendlyName();

	Type GetInType();

	Type GetOutType();

	void ShiftReferenceArgumentPositions(Guid tableId, Size size, Position? referencePosition = null, bool ignoreLocking = false)
	{
		ReferenceArguments.Where(a =>
			a.Reference.ReferencedTableId == tableId).ForEach(a =>
				a.Reference.ShiftReferencedPositions(size, referencePosition, ignoreLocking));
	}
}