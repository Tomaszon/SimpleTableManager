using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models;

public class GroupedObjectFunctions
{
	public Position Position { get; set; }

	public List<ObjectFunction> Values { get; set; }

	public GroupedObjectFunctions(Position position, IEnumerable<ObjectFunction> values)
	{
		Position = position;
		Values = values.ToList();
	}
}