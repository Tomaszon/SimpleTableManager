using System.Runtime.CompilerServices;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class CommandInformationAttribute : Attribute
{
	public string Information { get; set; }

	public CommandInformationAttribute(string information)
	{
		Information = information;
	}
}


public class CommandInformationAttribute<T> : CommandInformationAttribute
{
	public CommandInformationAttribute([CallerMemberName] string method = null!) : base(null!)
	{
		if (Localizer.TryLocalize<T>(method, "info", out var information))
		{
			Information = information;
		}
		else
		{
			Information = $"Translation not found: {information}";
		}
	}
}