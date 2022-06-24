using System.Collections.Generic;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	string TypeName { get; }

	List<Position> GetReferredCellPositions();

	List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null);

	void AddArguments(IEnumerable<object> arguments);

	void InsertArguments(int index,  IEnumerable<object> arguments);

	void RemoveLastArgument();
}