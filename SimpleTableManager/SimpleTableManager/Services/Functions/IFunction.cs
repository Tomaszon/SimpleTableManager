using System.Collections.Generic;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	string TypeName { get; }

	List<Position> GetReferredCellPositions();

	List<ObjectFunction> Execute(IEnumerable<GroupedObjectFunctions> parameters = null);

	void AddArguments(IEnumerable<object> arguments);

	void InsertArguments(int index,  IEnumerable<object> arguments);

	void RemoveLastArgument();
}