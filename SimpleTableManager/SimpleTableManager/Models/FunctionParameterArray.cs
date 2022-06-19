using System.Collections.Generic;

namespace SimpleTableManager.Models;

public class FunctionParameterArray
{
	public Position Position { get; set; }

	public List<FunctionParameter> Parameters { get; set; }

	public FunctionParameterArray(Position position, List<FunctionParameter> parameters)
	{
		Position = position;
		Parameters = parameters;
	}
}