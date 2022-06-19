using System.Collections.Generic;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public interface IFunction
{
	List<Position> GetReferredCellPositions();

	FunctionParameter Execute(IEnumerable<FunctionParameterArray> parameters = null);
}