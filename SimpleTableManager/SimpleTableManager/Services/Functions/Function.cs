using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public abstract class Function
{
	public abstract FunctionParameter Execute(IEnumerable<FunctionParameter> parameters = null);

	public List<Position> GetReferredCellPositions() => Arguments.Where(a => a.ReferencePosition is not null).Select(a => a.ReferencePosition).ToList();

	public List<FunctionParameter> Arguments { get; init; }
}